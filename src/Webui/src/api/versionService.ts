import { Version } from "../types/types";
import { del, get } from "./apiClient";

export async function fetchAppVersions(appId: number): Promise<Version[]>{
    return get<Version[]>(`application/${appId}/list`);
}

export async function deleteVersionById(versionId: number): Promise<unknown>{
    return del<unknown>(`version/${versionId}`);
}