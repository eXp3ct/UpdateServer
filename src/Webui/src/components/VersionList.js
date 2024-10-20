import React, { useState, useEffect } from 'react';
import { ListGroup, Row, Col, Modal } from 'react-bootstrap';
import { getAppVersions, getVersionInfoById } from '../api/api';
import './components.css'; // Тут можно добавить доп. стили

function VersionList({ app }) {
    const [selectedVersion, setSelectedVersion] = useState(null);
    const [versions, setVersions] = useState([]);

    // Загрузка версий
    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await getAppVersions(app);
                const sortedVersions = response.sort(
                    (a, b) => new Date(b.releaseDate) - new Date(a.releaseDate)
                );
                setVersions(sortedVersions);
            } catch (error) {
                console.error('Error fetching app versions:', error);
            }
        };

        fetchData();
    }, [app]);

    const handleVersionClick = async (id) => {
        const versionInfo = await getVersionInfoById(id);
        setSelectedVersion(versionInfo);
    };

    const handleClose = () => setSelectedVersion(null);

    return (
        <>
            <ListGroup className="my-3 version-list">
                {versions.map((version, index) => (
                    <ListGroup.Item
                        key={version.id}
                        onClick={() => handleVersionClick(version.id)}
                        className="version-item"
                    >
                        {version.version} (
                        {new Intl.DateTimeFormat('ru-RU', {
                            dateStyle: 'full',
                        }).format(new Date(version.releaseDate))}
                        )
                        {index === 0 && (
                            <span className="badge bg-success ms-2">Последняя</span>
                        )}
                        {version.isMandatory && (
                            <span className="badge bg-warning ms-2">Обязательна</span>
                        )}
                        {!version.isActive && (
                            <span className="badge bg-danger ms-2">Доступна</span>
                        )}
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
                                <Col xs={7} className="value">{selectedVersion.applicationName}</Col>
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
                                        href={selectedVersion.changelogFileUrl}
                                        target="_blank"
                                        rel="noopener noreferrer"
                                        className="link"
                                    >
                                        {selectedVersion.changelogFileUrl}
                                    </a>
                                </Col>
                            </Row>
                            <Row className="mb-2">
                                <Col xs={5} className="label">Файл сборки:</Col>
                                <Col xs={7} className="value">
                                    <a
                                        href={selectedVersion.zipUrl}
                                        target="_blank"
                                        rel="noopener noreferrer"
                                        className="link"
                                    >
                                        {selectedVersion.zipUrl}
                                    </a>
                                </Col>
                            </Row>
                            <Row className="mb-2">
                                <Col xs={5} className="label">Обязательно:</Col>
                                <Col xs={7} className="value">
                                    <span className="badge bg-warning ms-2">{selectedVersion.isMandatory ? 'Да' : 'Нет'}</span>
                                </Col>
                            </Row>
                            <Row className="mb-2">
                                <Col xs={5} className="label">Доступна:</Col>
                                <Col xs={7} className="value">
                                    <span className="badge bg-danger ms-2">{selectedVersion.isActive ? 'Да' : 'Нет'}</span>
                                </Col>
                            </Row>
                        </div>
                    )}
                </Modal.Body>
            </Modal>
        </>
    );
}

export default VersionList;
