import express from 'express';
import passport from 'passport';
import session from 'express-session';
import GoogleStrategy from 'passport-google-oauth20';
import { Profile } from 'passport-google-oauth20';
import dotenv from 'dotenv';

dotenv.config();

const app = express();
const port = 4000;

app.use(express.static('src'));

app.use(session({
  secret: 'your-secret-value',
  resave: false,
  saveUninitialized: false,
}));

// Register Google Strategy
passport.use(
  new GoogleStrategy.Strategy(
    {
      clientID: process.env.GOOGLE_CLIENT_ID as string,
      clientSecret: process.env.GOOGLE_CLIENT_SECRET as string,
      callbackURL: 'http://localhost:4000/auth/google/callback',
    },
    (accessToken: string, refreshToken: any, profile: Profile, done: (error: any, user?: any, info?: any) => void) => {
      // Here, you should save user data to a database and call done()
      done(null, profile);
    }
  )
);

app.use(passport.initialize());
app.use(passport.session());

passport.serializeUser((user: any, done) => {
  done(null, user.id);
});

passport.deserializeUser((id, done) => {
  done(null, { id: id });
});

// Google authentication route
app.get(
  '/auth/google',
  passport.authenticate('google', {
    scope: ['profile', 'email'],
  })
);

// Google authentication callback route
app.get(
  '/auth/google/callback',
  passport.authenticate('google', { failureRedirect: '/login' }),
  (req, res) => {
    // Successful authentication, redirect home.
    res.redirect('/');
  }
);

// Login route
app.get('/login', (req, res) => {
  res.sendFile(__dirname + '/src/login.html');
});


app.listen(4000, () => {
  console.log('Server is running on port 4000');
});