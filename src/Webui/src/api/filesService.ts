import { del, post } from "./apiClient";

export async function uploadFile(file: File, versionId: number, type: number): Promise<any> {
    
    let formData = new FormData();
    formData.append('file', file);

    return post<any>(`files/${versionId}?type=${type}`, formData, {
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    });
}

export async function deleteFiles(versionId: number): Promise<any> {
    return del<any>(`files/${versionId}`);
}