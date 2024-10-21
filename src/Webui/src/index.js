import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
// Bootstrap CSS
import "bootstrap/dist/css/bootstrap.min.css";
// Bootstrap Bundle JS
import "bootstrap/dist/js/bootstrap.bundle.min";
import { loadConfig } from './api/api';

const renderApp = async () => {
  try {
    await loadConfig; // Загружаем конфигурацию перед рендером
    ReactDOM.render(
      <React.StrictMode>
        <App />
      </React.StrictMode>,
      document.getElementById('root')
    );
  } catch (error) {
    console.error('Ошибка при запуске приложения:', error);
  }
};

renderApp();

