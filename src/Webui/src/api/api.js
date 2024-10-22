import axios from 'axios';

const API_URL = window.API_URL;

export const fetchAppNames = async () => {

    try {
        const response = await axios.get(`${API_URL}/versions/apps`, {
            headers: {
                'Content-Type': 'application/json',
            },
        });
        return response.data;
    } catch (error) {
        console.error('Ошибка при загрузке приложений:', error);
        throw error;
    }
};

export const createVersion = async (versionData) => {
    try {
        const response = await axios.post(`${API_URL}/versions`, versionData, {
            headers: {
                'Content-Type': 'application/json',
            },
        });
        return response.data;
    } catch (error) {
        console.error('Ошибка при создании версии:', error);
        throw error;
    }
};

export const uploadFileToAPI = async (file, versionId, type) => {
    const formData = new FormData();
    formData.append('file', file);

    try {
        await axios.post(
            `${API_URL}/files?versionId=${versionId}&type=${type}`,
            formData,
            {
                headers: {
                    'Content-Type': 'multipart/form-data',
                },
            }
        );
    } catch (error) {
        console.error(`Ошибка при загрузке файла (${type}):`, error);
        throw error;
    }
};

export const getAppVersions = async (appName) => {
    try {
        const response = await axios.get(`${API_URL}/versions/${appName}/list`, {
            headers: {
                'Content-Type': 'application/json'
            }
        });

        return response.data
    } catch (error) {
        console.error('Error fetching app versions', error);
        throw error;
    }
}

export const getVersionInfoById = async (versionId) => {
    try {
        const response = await axios.get(`${API_URL}/versions/${versionId}/info`, {
            headers: {
                'Content-Type': 'application/json'
            }
        });

        return response.data
    } catch (error) {
        console.error('Error fetching app version info', error);
        throw error;
    }
} 

export const deleteVersionById = async (versionId) => {
    try{
        const response = await axios.delete(`${API_URL}/versions/${versionId}`);

        return response.data;
    }catch(error){
        console.error('Error deleting app version', error)
        throw error;
    }
}