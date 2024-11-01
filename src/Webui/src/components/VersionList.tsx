import React, { useEffect, useState } from 'react';
import { ListGroup, Button, Modal, Row, Col } from 'react-bootstrap';
import { Version, Application } from '../types/types';
import { createVersion, deleteVersionById, fetchAppVersions, getVersionById } from '../api/versionService';
import './components.css';
import AddVersionModal from './AddVersionModal';
import { uploadFile } from '../api/filesService';


interface Props {
    selectedApp: Application;
}

const VersionList: React.FC<Props> = ({ selectedApp }) => {
    const [showModal, setShowModal] = useState<boolean>(false);
    const [versions, setVersions] = useState<Version[]>([]);
    const [selectedVersion, setSelectedVersion] = useState<Version | null>(null);
    const [newVersion, setNewVersion] = useState<Version>({
        applicationId: 0,
        changelogUrl: '',
        id: 0,
        isAvailable: false,
        isMandatory: false,
        releaseDate: '',
        releaseUrl: '',
        version: ''
    });

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

    const handleVersionClick = async (versionId: number) => {
        const version = await getVersionById(versionId);
        setSelectedVersion(version);
    }

    const handleClose = () => setSelectedVersion(null);

    const handleAddVersion = async (version: Partial<Version>) => {
        const createdVersion = await createVersion({applicationId: selectedApp.id, ...version});

        console.log(createdVersion);

        setNewVersion(createdVersion);
        setVersions([newVersion, ...versions]);
    }

    const handleUploadFiles = async (file: File, type: number) => {
        await uploadFile(file, newVersion.id, type);
    }

    return (
        <div>
            <div className="d-flex justify-content-between align-items-center mb-3">
                <h3>Версии для {selectedApp.name}</h3>
                <Button variant="success" onClick={() => setShowModal(true)}>
                    Добавить версию
                </Button>
            </div>
            <ListGroup>
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
            </ListGroup>

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
                onUploadFile={handleUploadFiles}
            />
        </div>
    );
};

export default VersionList;