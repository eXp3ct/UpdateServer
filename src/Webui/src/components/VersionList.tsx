import React, { useEffect, useState } from 'react';
import { ListGroup, Button, Modal, Row, Col } from 'react-bootstrap';
import { Version, Application } from '../types/types';
import { createVersion, deleteVersionById, fetchAppVersions, getLatestVersion, getVersionById } from '../api/versionService';
import './components.css';
import AddVersionModal from './AddVersionModal';
import { deleteFiles, uploadFile } from '../api/filesService';


interface Props {
    selectedApp: Application;
}

const VersionList: React.FC<Props> = ({ selectedApp }) => {
    const [showModal, setShowModal] = useState<boolean>(false);
    const [versions, setVersions] = useState<Version[]>([]);
    const [selectedVersion, setSelectedVersion] = useState<Version | null>(null);
    const [latestVersion, setLatestVersion] = useState<Version>();

    useEffect(() => {
        const loadVersions = async (appId: number) => {
            try {
                let versions = await fetchAppVersions(appId);
        
                // Сортируем версии по убыванию, сравнивая номера версии как числа
                versions = versions.sort((a, b) => {
                    const versionA = a.version.split('.').map(Number);
                    const versionB = b.version.split('.').map(Number);
                    for (let i = 0; i < versionA.length; i++) {
                        if ((versionB[i] || 0) - (versionA[i] || 0) !== 0) {
                            return (versionB[i] || 0) - (versionA[i] || 0);
                        }
                    }
                    return 0;
                });
        
                setVersions(versions);
            } catch (error) {
                console.error('Error fetching versions', error);
            }
        }
        const getLatest = async (appName: string) => {
            try{
                const latest = await getLatestVersion(appName);
                setLatestVersion(latest);
            }catch(error){
                console.error('Error fetching latest version', error);
            }
        }

        loadVersions(selectedApp.id);
        getLatest(selectedApp.name);

    }, [selectedApp]);

    const handleDelete = async (versionId: number) => {
        await deleteFiles(versionId);
        await deleteVersionById(versionId);
        setVersions((prev) => prev.filter((v) => v.id !== versionId));
    }

    const handleVersionClick = async (versionId: number) => {
        const version = await getVersionById(versionId);
        setSelectedVersion(version);
    }

    const handleClose = () => setSelectedVersion(null);

    const handleAddVersion = async (version: Partial<Version>, changelogFile?: File, releaseFile?: File) => {
        const createdVersion = await createVersion({ applicationId: selectedApp.id, ...version });
    
        if (changelogFile) {
            await uploadFile(changelogFile, createdVersion.id, 0);
        }
        if (releaseFile) {
            await uploadFile(releaseFile, createdVersion.id, 1);
        }
    
        setVersions([createdVersion, ...versions]);
    };

    return (
        <div>
            <div className="d-flex justify-content-between align-items-center mb-3">
                <h3>Версии для {selectedApp.name}</h3>
                <Button variant="success" onClick={() => setShowModal(true)}>
                    Добавить версию
                </Button>
            </div>
            {versions.length <= 0 
            ? 
            <h5>
                Нет сохранненых версий
            </h5> 
            : (<ListGroup>
                {versions.map(version => (
                    <ListGroup.Item 
                    key={version.id} 
                    className="d-flex justify-content-between align-items-center version-item"
                    onClick={() => handleVersionClick(version.id)}>
                        <div>
                            <h5>Версия {version.version}</h5>
                            <small>Дата релиза: {new Intl.DateTimeFormat('ru-RU', {
                                dateStyle: 'full',
                            }).format(new Date(version.releaseDate))}</small>
                            {version.id === latestVersion?.id && (
                                <span className="badge bg-success ms-2">Последняя</span>
                            )}
                            {version.isMandatory && (
                                <span className="badge bg-warning ms-2">Обязательна</span>
                            )}
                            {!version.isAvailable && (
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
            </ListGroup>)}

            <Modal show={!!selectedVersion} onHide={handleClose} centered>
                <Modal.Header closeButton>
                    <Modal.Title>Полная информация о версии</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    {selectedVersion && (
                        <div className="version-info">
                            <Row className="mb-2">
                                <Col xs={5} className="label">Приложение:</Col>
                                <Col xs={7} className="value">{selectedApp.name}</Col>
                            </Row>
                            <Row className="mb-2">
                                <Col xs={5} className="label">Версия:</Col>
                                <Col xs={7} className="value">{selectedVersion.version}</Col>
                            </Row>
                            <Row className="mb-2">
                                <Col xs={5} className="label">Дата выпуска:</Col>
                                <Col xs={7} className="value">
                                    {new Intl.DateTimeFormat('ru-RU', { dateStyle: 'full' }).format(
                                        new Date(selectedVersion.releaseDate)
                                    )}
                                </Col>
                            </Row>
                            <Row className="mb-2">
                                <Col xs={5} className="label">Файл изменений:</Col>
                                <Col xs={7} className="value">
                                    <a
                                        href={selectedVersion.changelogUrl}
                                        target="_blank"
                                        rel="noopener noreferrer"
                                        className="link"
                                    >
                                        {selectedVersion.changelogUrl ? 'changelog.html' : 'Нет'}
                                    </a>
                                </Col>
                            </Row>
                            <Row className="mb-2">
                                <Col xs={5} className="label">Файл сборки:</Col>
                                <Col xs={7} className="value">
                                    <a
                                        href={selectedVersion.releaseUrl}
                                        target="_blank"
                                        rel="noopener noreferrer"
                                        className="link"
                                    >
                                        {selectedVersion.releaseUrl ? 'release.zip' : 'Нет'}
                                    </a>
                                </Col>
                            </Row>
                            <Row className="mb-2">
                                <Col xs={5} className="label">Обязательно:</Col>
                                <Col xs={7} className="value">
                                    {selectedVersion.isMandatory ? 'Да' : 'Нет'}
                                </Col>
                            </Row>
                            <Row className="mb-2">
                                <Col xs={5} className="label">Доступна:</Col>
                                <Col xs={7} className="value">
                                    {selectedVersion.isAvailable ? 'Да' : 'Нет'}
                                </Col>
                            </Row>
                        </div>
                    )}
                </Modal.Body>
            </Modal>
            
            <AddVersionModal
                show={showModal}
                onHide={() => setShowModal(false)}
                onAdd={handleAddVersion}
            />
        </div>
    );
};

export default VersionList;