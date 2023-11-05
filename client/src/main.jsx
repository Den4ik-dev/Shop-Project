import React from 'react';
import ReactDOM from 'react-dom/client';
import Router from './components/Router';
import './global.css';
import { ApplicationProvider } from './providers/ApplicationProvider';

ReactDOM.createRoot(document.getElementById('root')).render(
  <ApplicationProvider>
    <Router />
  </ApplicationProvider>
);
