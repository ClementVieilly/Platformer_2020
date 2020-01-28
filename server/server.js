const express = require("express");
const ObjectId = require("mongoose").Types.ObjectId;
const bcrypt = require("bcrypt");
const jwt = require("jsonwebtoken");
const mysql = require('mysql2');
const app = express();

const jsonMiddleware = express.json();
app.use(jsonMiddleware);

// Le secret avec lequel les JWT sont signés
const secret = process.env.JWT_SECRET;

// Ce middleware vérifie qu’un JWT valide est présent dans le header Authorization
const jwtMiddleware = require("express-jwt")({
  secret: secret
// sauf pour les routes permettant à un joueur de récupérer un JWT.
}).unless({ path: ["/users/signup", "/users/signin"] });
app.use(jwtMiddleware);

// Crée le pool de connexions à la database
const pool = mysql.createPool({
  host: process.env.DATABASE_HOST,
  user: process.env.DATABASE_USER,
  password: process.env.DATABASE_PASSWORD,
  database: process.env.DATABASE_NAME,
  waitForConnections: true,
  connectionLimit: 10,
  queueLimit: 0
});

//const promisePool = pool.promise();

// POST /users/signup
app.post("/users/signup", function (req, res) {
  const username = req.body.username;
  let userAlreadyRegistered = false;

  // Vérifie dans la base de données si un compte existe déjà pour cet username.
  pool.execute(
    "SELECT * FROM users WHERE username = ?",
    [username],
    function (err, results, fields) {
      if (err) console.log(err);

      if (results.length) userAlreadyRegistered = true;
    }
  );

  // 409 (Conflict), ce code signifie qu’un compte existe déjà pour cet username.
  if (userAlreadyRegistered)
  {
    console.log("This user already exist: " + username);
    return res.sendStatus(409);
  }

  // Le mot de passe du joueur est transmis tel quel via HTTPS
  // et chiffré avec `bcrypt`.
  bcrypt.hash(req.body.password, 21, function (err, password) {
    if (err) return next(err);

    /*const id = ObjectId();
    users[username] = {
      id: id,
      username: username,
      password: password
    };*/

    pool.execute(
      "INSERT INTO users (username, password) values (?, ?)",
      [username, password],
      function (err, results, fields) {
        if (err) console.log(err);
      }
    );

    res.send(200);
    // Une fois le compte du joueur créé, il faut lui retourner un
    // token lui permettant d’authentifier ses requêtes suivantes.
    /*jwt.sign({ id: id }, secret, function (err, token) {
      if (err) return next(err);
      res.send(token);
    });*/
  });
});

// POST /users/signin
app.post("/users/signin", function (req, res) {
  const username = req.body.username;
  const password = req.body.password;
  const user = users[username];

  // 404 (Not Found), ce code signife qu’aucun compte n’existe pour cet username.
  if (!user) return res.sendStatus(404);

  bcrypt.compare(password, user.password, function (err, same) {
    if (err) return next(err);

    // 401 (Unauthorized), ce code signifie que le client n’a pas les
    // autorisations nécessaires pour cette requête (le mot de passe ne correspond pas).
    if (!same) return res.sendStatus(401);

    // Une fois l’identité du joueur vérifiée, il faut retourner le
    // token lui permettant d’authentifier ses requêtes suivantes.
    jwt.sign({ id: user.id }, secret, function (err, token) {
      if (err) return next(err);
      res.send(token);
    });
  });
});

// Les routes suivantes sont protégées par `express-jwt`: elles
// n’acceptent que les requêtes dont le header Authorization contient
// un JWT signé par `process.env.JWT_SECRET`.

// Le contenu déchiffré du token est accessible dans la propriété
// `user` de la requête. Il faut ensuite vérifier que ce token a les
// autorisations nécessaires en comparant les informations qu’il
// contient avec les resources demandées ou modifiées par les requêtes.

// GET /users/me
// On peut aussi utiliser l’identifiant présent dans le token pour
// retourner des informations sur le client.
app.get("/users/me", function (req, res) {
  res.sendStatus(200);
});

// GET /users/:userId
app.get("/users/:userId", function (req, res) {
  if (req.user.id === req.params.userId) {
    res.sendStatus(200);
  } else {
    // 403 (Forbidden), ce code signifie que le client n’a pas le droit
    // d’exécuter cette requête.
    res.sendStatus(403);
  }
});

// Requète inexistante : Afficher erreur 404 au lieu d'une page HTML dans la console Unity
app.use(function(req, res, next) {
    res.sendStatus(404);
});

// Erreur interne du server : Afficher la stack d'erreur au lieu d'une page HTML dans la console Unity
app.use(function(err, req, res, next) {
  res.status(500).send(err.stack);
});

const port = process.env.DATABASE_PORT || 8000;
app.listen(port, function (err) {
  if (err) console.error(err);
  else console.log("Listening to https://platformer-sequoia.herokuapp.com/:" + port);
});