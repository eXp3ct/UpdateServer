interface EnvConfig {
    apiUrl: string;
    environment: string;
}

export const getEnvConfig = (): EnvConfig => {
    return {
        apiUrl: process.env.API_URL || 'http://localhost:5069/api/',
        environment: process.env.NODE_ENV || 'development'
    };
};

// Создаем конфиг, который можно импортировать
export const envConfig = getEnvConfig();