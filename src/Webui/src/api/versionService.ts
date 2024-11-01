import { Version } from "../types/types";
import { del, get, post } from "./apiClient";

export async function fetchAppVersions(appId: number): Promise<Version[]>{
    return get<Version[]>(`application/${appId}/list`);
}

export async function deleteVersionById(versionId: number): Promise<any>{
    return del<any>(`version/${versionId}`);
}

export async function getVersionById(versionId: number): Promise<Version>{
    return get<Version>(`version/${versionId}`);
}

export async function createVersion(verion: Partial<Version>): Promise<Version> {
    return post<Version>(`version`, verion);
}