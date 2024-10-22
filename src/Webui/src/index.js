import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap/dist/js/bootstrap.bundle.min";

const renderApp = async () => {

  // try {
  //   await loadConfig(); // Загружаем конфигурацию перед рендером
    
  // } catch (error) {
  //   console.error('Ошибка при запуске приложения:', error);
  // }

  const root = ReactDOM.createRoot(document.getElementById('root'));
  root.render(
    <React.StrictMode>
      <App />
    </React.StrictMode>  
  );
  
};
renderApp();

