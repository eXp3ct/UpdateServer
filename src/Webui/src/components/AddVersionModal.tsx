import React, { useState } from 'react';
import { Modal, Button, Form } from 'react-bootstrap';
import { Version } from '../types/types';

interface Props {
    show: boolean;
    onHide: () => void;
    onAdd: (version: Partial<Version>) => void;
    onUploadFile: (file: File, type: number) => void;
}

const AddVersionModal: React.FC<Props> = ({ show, onHide, onAdd, onUploadFile }) => {

    const [versionNumber, setVersionNumber] = useState<string>('');
    const [isMandatory, setIsMandatory] = useState<boolean>(false);
    const [isAvailable, setIsAvailable] = useState<boolean>(true);
    const [changelogFile, setChangelogFile] = useState<File | null>(null);
    const [releaseFile, setReleaseFile] = useState<File | null>(null);

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();

        // Здесь передаем основные данные версии
        onAdd({ version: versionNumber, isAvailable: isAvailable, isMandatory: isMandatory });

        // Вызов функции загрузки файлов (например, после успешного добавления версии)
        if (changelogFile) {
            onUploadFile(changelogFile, 0); // Передать реальный ID версии и тип файла
        }
        if (releaseFile) {
            onUploadFile(releaseFile, 1); // Передать реальный ID версии и тип файла
        }

        // Очистка формы после отправки
        setVersionNumber('');
        setIsAvailable(false);
        setIsMandatory(true);
        setChangelogFile(null);
        setReleaseFile(null);

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
                                const target = e.target as HTMLInputElement; // Приведение к HTMLInputElement
                                setChangelogFile(target.files ? target.files[0] : null);
                            }}
                            required
                        />
                    </Form.Group>
                    <Form.Group className="mb-3">
                        <Form.Label>Файл релиза</Form.Label>
                        <Form.Control
                            type="file"
                            onChange={(e) => {
                                const target = e.target as HTMLInputElement; // Приведение к HTMLInputElement
                                setReleaseFile(target.files ? target.files[0] : null);
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
