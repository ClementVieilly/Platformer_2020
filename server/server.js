const express = require("express");
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
  port: process.env.DATABASE_PORT,
  waitForConnections: true,
  connectionLimit: 10,
  queueLimit: 0
});

//const promisePool = pool.promise();

// POST /users/signup
app.post("/users/signup", function (req, res, next) {
  const username = req.body.username;

  // Vérifie dans la base de données si un compte existe déjà pour cet username.
  pool.execute(
    "SELECT * FROM users WHERE username = ?",
    [username],
    function (err, results, fields) {
      if (err)
        return next(err);

      if (results && results.length)
      {
        const msg = "This user already exist: " + username;
        console.log(msg);
        return res.status(409).send(msg);
      }

      // Le mot de passe du joueur est transmis tel quel via HTTPS
      // et chiffré avec `bcrypt`.
      bcrypt.hash(req.body.password, 10, function (err, password) {
        if (err) return next(err);

        pool.execute(
          "INSERT INTO users (username, password) VALUES (?, ?)",
          [username, password],
          function (err, results, fields) {
            if (err) return next(err);

            pool.execute(
              "SELECT user_id FROM users WHERE username = ?",
              [username],
              function (err, results, fields) { 
                // Une fois le compte du joueur créé, il faut lui retourner un
                // token lui permettant d’authentifier ses requêtes suivantes.
                jwt.sign({ id: results[0].user_id }, secret, function (err, token) {
                  if (err) return next(err);
                  res.send(token);
                });
              }
            );
          }
        );
      });
    }
  );
});

// POST /users/signin
app.post("/users/signin", function (req, res, next) {
  res.sendStatus(200);
});

// Requète inexistante : Afficher erreur 404 au lieu d'une page HTML dans la console Unity
app.use(function(req, res, next) {
    res.sendStatus(404);
});

// Erreur interne du server : Afficher la stack d'erreur au lieu d'une page HTML dans la console Unity
app.use(function(err, req, res, next) {
  console.log(err);
  res.status(500).send(err.stack);
});

const port = process.env.PORT || 8000;
app.listen(port, function (err) {
  if (err) console.error(err);
  else console.log("Listening to " + process.env.DATABASE_HOST + ":" + port);
});