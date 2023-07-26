import express from 'express';
import passport from 'passport';
import session, { SessionData } from 'express-session';
import GoogleStrategy from 'passport-google-oauth20';
import { Profile } from 'passport-google-oauth20';
import dotenv from 'dotenv';

dotenv.config();

// Declare merged types here
declare module 'express-session' {
  interface SessionData {
    passport?: {
      user?: any;
    };
  }
}

const app = express();
const port = 4000;

app.use(express.static('src'));

app.use(session({
  secret: 'your-secret-value',
  resave: false,
  saveUninitialized: false,
}));

function ensureAuthenticated(req: express.Request, res: express.Response, next: express.NextFunction) {
  if (req.isAuthenticated()) {
    return next();
  }
  res.redirect('/login');
}

// Register Google Strategy
passport.use(
  new GoogleStrategy.Strategy(
    {
      clientID: process.env.GOOGLE_CLIENT_ID as string,
      clientSecret: process.env.GOOGLE_CLIENT_SECRET as string,
      callbackURL: 'http://localhost:4000/auth/google/callback',
    },
    (accessToken: string, refreshToken: any, profile: Profile, done: (error: any, user?: any, info?: any) => void) => {
      const user = {
        id: profile.id,
        email: profile.emails ? profile.emails[0].value : null
      };
      done(null, user);
    }
  )
);


app.use(passport.initialize());
app.use(passport.session());

passport.serializeUser((user: any, done) => {
  done(null, user);
});

passport.deserializeUser((user: any, done) => {
  done(null, user);
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

app.get('/some-route', ensureAuthenticated, (req: express.Request, res: express.Response) => {
  // This code will only be executed if the user is authenticated.
  // Here we are asserting that req.user will exist
  if(req.session && req.session.passport && req.session.passport.user){
    console.log(req.session.passport.user.email);
  } 
});



app.listen(4000, () => {
  console.log('Server is running on port 4000');
});