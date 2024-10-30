import { Application } from '../types/types';
import { get, post } from './apiClient';

export async function fetchApps(): Promise<Application[]>{
    return get<Application[]>('application');
}

export async function createApp(app: Partial<Application>): Promise<Application>{
    return post<Application>('application', app);
}
