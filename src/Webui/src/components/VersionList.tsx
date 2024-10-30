import React, { useEffect, useState } from 'react';
import { ListGroup, Button } from 'react-bootstrap';
import { Version, Application } from '../types/types';
import { deleteVersionById, fetchAppVersions } from '../api/versionService';

interface Props {
    selectedApp: Application;
    onAddVersion: (app: Application) => void;
}

const VersionList: React.FC<Props> = ({ selectedApp, onAddVersion }) => {

    const [versions, setVersions] = useState<Version[]>([]);

    useEffect(() => {
        const loadVersions = async (appId: number) => {
            try {
                const versions = await fetchAppVersions(appId);
                setVersions(versions);
            } catch (error) {
                console.error('Error fetching versions', error);
            }
        }
        loadVersions(selectedApp.id);
    }, [selectedApp]);

    const handleDelete = async (versionId: number) => {
        await deleteVersionById(versionId);
        setVersions((prev) => prev.filter((v) => v.id !== versionId));
    }

    return (
        <div>
            <div className="d-flex justify-content-between align-items-center mb-3">
                <h3>Версии для {selectedApp.name}</h3>
                <Button variant="success" onClick={() => onAddVersion(selectedApp)}>
                    Добавить версию
                </Button>
            </div>
            <ListGroup>
                {versions.map(version => (
                    <ListGroup.Item key={version.id} className="d-flex justify-content-between align-items-center">
                        <div>
                            <h5>Версия {version.version}</h5>
                            <small>Дата релиза: {new Intl.DateTimeFormat('ru-RU', {
                                dateStyle: 'full',
                            }).format(new Date(version.releaseDate))}</small>
                            {version.isMandatory && (
                                <span className="badge bg-warning ms-2">Обязательна</span>
                            )}
                            {version.isAvailable && (
                                <span className="badge bg-danger ms-2">Не доступна</span>
                            )}
                        </div>

                        <Button
                            variant="outline-danger"
                            size="sm"
                            className="ms-3"
                            onClick={(e) => {
                                e.stopPropagation(); // Останавливаем всплытие события
                                handleDelete(version.id);
                            }}
                        >
                            <i className="bi bi-trash"></i>
                        </Button>
                    </ListGroup.Item>
                ))}
            </ListGroup>
        </div>
    );
};

export default VersionList;