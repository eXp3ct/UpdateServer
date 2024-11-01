export interface Application {
    id: number;
    name: string;
    description: string;
    dateOfCreation: string;
    dateModified: string;
}

export interface Version {
    id: number;
    applicationId: number;
    version: string;
    releaseDate: string;
    isMandatory: boolean;
    isAvailable: boolean;
    changelogUrl: string;
    releaseUrl: string;
}