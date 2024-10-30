import { Application, Version } from '../types/types';

export const mockApplications: Application[] = [
    { id: 1, name: "App 1", description: "Description for App 1", dateOfCreation: '', dateModified: '' },
    { id: 2, name: "App 2", description: "Description for App 2", dateOfCreation: '', dateModified: '' },
    { id: 3, name: "App 3", description: "Description for App 3", dateOfCreation: '', dateModified: '' },
];

export const mockVersions: Version[] = [
    { id: 1, applicationId: 1, version: "1.0.0.0", releaseDate: "2024-01-01", isAvailable: true, isMandatory: true },
    { id: 2, applicationId: 1, version: "1.1.0.0", releaseDate: "2024-02-01", isAvailable: false, isMandatory: true },
    { id: 3, applicationId: 2, version: "1.0.0.0", releaseDate: "2024-01-15", isAvailable: true, isMandatory: false },
    { id: 4, applicationId: 3, version: "1.2.0.0", releaseDate: "2024-01-15", isAvailable: false, isMandatory: false },
];