import axios from "axios"

const API_URL = "http://localhost:5069";

export const getAppNames = async () => {
    try{
        const response = await axios.get(`${API_URL}/versions/apps`, {
            headers: {
                'Content-Type': 'application/json'
            }
        });

        return response.data;
    }catch(error){
        console.error('Error fetching app names', error);
        throw error;
    }
}

export const getAppVersions = async (appName) => {
    try{
        const response = await axios.get(`${API_URL}/versions/${appName}/list`, {
            headers: {
                'Content-Type': 'application/json'
            }
        });

        return response.data
    }catch(error){
        console.error('Error fetching app versions', error);
        throw error;
    }
}

export const getVersionInfo = async (appName) => {
    try{
        const response = await axios.get(`${API_URL}/versions/${appName}`, {
            headers: {
                'Content-Type': 'application/json'
            }
        });

        return response.data
    }catch(error){
        console.error('Error fetching app version info', error);
        throw error;
    }
}