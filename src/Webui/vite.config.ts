import { defineConfig, loadEnv } from 'vite';
import react from '@vitejs/plugin-react';
import { resolve } from 'path';

// https://vite.dev/config/

export default defineConfig(({ mode }) => {
  // Загружаем переменные окружения в зависимости от mode
  const env = loadEnv(mode, process.cwd(), '');
  
  return {
    plugins: [react()],
    server: {
      port: 3000,
    },
    build: {
      rollupOptions: {
        input: {
          main: resolve(__dirname, 'index.html'),
        },
      },
    },
    define: {
      // Передаем переменные окружения в приложение
      'process.env.API_URL': JSON.stringify(env.API_URL || 'http://localhost:5069/api/'),
      'process.env.NODE_ENV': JSON.stringify(mode)
    }
  };
});