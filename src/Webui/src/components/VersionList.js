import React, { useState, useEffect } from 'react';
import { ListGroup, Button, Modal } from 'react-bootstrap';
import { getAppVersions, getVersionInfo } from '../api/api';

// const versions = [
//     { id: '1', releaseDate: '2024-01-01', version: '1.0.0.0', isMandatory: true },
//     { id: '2', releaseDate: '2024-02-15', version: '1.1.0.0', isMandatory: false },
// ];

function VersionList({ app }) {
    const [selectedVersion, setSelectedVersion] = useState(null);
    const [versions, setVersions] = useState([])

    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await getAppVersions(app);
                setVersions(response);
            } catch (error) {
                console.error('Error fetching app names:', error);
            }
        };

        fetchData();
    }, []);

    const handleVersionClick = async (version) => {
        const versionInfo = await getVersionInfo(app);
        setSelectedVersion(versionInfo);
    };

    const handleClose = () => setSelectedVersion(null);


    return (
        <>
            <ListGroup className="my-3">
                {versions.map((version) => (
                    <ListGroup.Item key={version.id} onClick={() => handleVersionClick(version)}>
                        {version.version} ({new Intl.DateTimeFormat('ru-RU', { dateStyle: 'full' }).format(new Date(version.releaseDate))})
                    </ListGroup.Item>
                ))}
                <Button variant="outline-primary" className="mt-2">
                    + Добавить версию
                </Button>
            </ListGroup>

            <Modal show={!!selectedVersion} onHide={handleClose}>
                <Modal.Header closeButton>
                    <Modal.Title>Полная информация о версии</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    {selectedVersion && (
                        <>
                            <p>Дата выпуска: {new Intl.DateTimeFormat('ru-RU', { dateStyle: 'full' }).format(new Date(selectedVersion.releaseDate))}</p>
                            <p>Версия: {selectedVersion.version}</p>
                            <p>Обязательно: {selectedVersion.isMandatory ? 'Да' : 'Нет'}</p>
                        </>
                    )}
                </Modal.Body>
            </Modal>
        </>
    );
}

export default VersionList;
