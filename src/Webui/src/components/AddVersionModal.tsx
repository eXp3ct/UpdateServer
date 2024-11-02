import React, { useState } from 'react';
import { Modal, Button, Form } from 'react-bootstrap';
import { Version } from '../types/types';

interface Props {
    show: boolean;
    onHide: () => void;
    onAdd: (version: Partial<Version>, changelogFile?: File, releaseFile?: File) => void;
}

const AddVersionModal: React.FC<Props> = ({ show, onHide, onAdd }) => {

    const [versionNumber, setVersionNumber] = useState<string>('');
    const [isMandatory, setIsMandatory] = useState<boolean>(false);
    const [isAvailable, setIsAvailable] = useState<boolean>(true);
    const [changelogFile, setChangelogFile] = useState<File>();
    const [releaseFile, setReleaseFile] = useState<File>();

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();

        onAdd(
            {
                version: versionNumber,
                isAvailable: isAvailable,
                isMandatory: isMandatory,
            },
            changelogFile,
            releaseFile
        );

        setVersionNumber('');
        setIsAvailable(false);
        setIsMandatory(true);
        setChangelogFile(undefined);
        setReleaseFile(undefined);

        onHide();
    };

    return (
        <Modal show={show} onHide={onHide}>
            <Modal.Header closeButton>
                <Modal.Title>Добавить новую версию</Modal.Title>
            </Modal.Header>
            <Form onSubmit={handleSubmit}>
                <Modal.Body>
                    <Form.Group className="mb-3">
                        <Form.Label>Номер версии</Form.Label>
                        <Form.Control
                            type="text"
                            value={versionNumber}
                            onChange={(e) => setVersionNumber(e.target.value)}
                            className="form-control"
                            pattern="\d\.\d\.\d\.\d"
                            placeholder="0.0.0.0"
                            required
                        />
                    </Form.Group>
                    <Form.Group className="mb-3">
                        <Form.Label>Обязательна</Form.Label>
                        <Form.Check
                            type="checkbox"
                            checked={isMandatory}
                            onChange={() => setIsMandatory(!isMandatory)}
                        />
                    </Form.Group>
                    <Form.Group className="mb-3">
                        <Form.Label>Доступна</Form.Label>
                        <Form.Check
                            type="checkbox"
                            checked={isAvailable}
                            onChange={() => setIsAvailable(!isAvailable)}
                        />
                    </Form.Group>
                    <Form.Group className="mb-3">
                        <Form.Label>Файл изменений</Form.Label>
                        <Form.Control
                            type="file"
                            onChange={(e) => {
                                const target = e.target as HTMLInputElement;
                                setChangelogFile(target.files ? target.files[0] : undefined);
                            }}
                            required
                        />
                    </Form.Group>
                    <Form.Group className="mb-3">
                        <Form.Label>Файл релиза</Form.Label>
                        <Form.Control
                            type="file"
                            onChange={(e) => {
                                const target = e.target as HTMLInputElement;
                                setReleaseFile(target.files ? target.files[0] : undefined);
                            }}
                            required
                        />
                    </Form.Group>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={onHide}>
                        Отмена
                    </Button>
                    <Button variant="primary" type="submit">
                        Добавить
                    </Button>
                </Modal.Footer>
            </Form>
        </Modal>
    );
};

export default AddVersionModal;
