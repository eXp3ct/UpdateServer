import { Application } from '../types/types';
import { del, get, post } from './apiClient';

export async function fetchApps(): Promise<Application[]>{
    return get<Application[]>('application');
}

export async function createApp(app: Partial<Application>): Promise<Application>{
    return post<Application>('application', app);
}

export async function deleteApp(appId: number): Promise<any>{
    return del<any>(`application/${appId}`);
}