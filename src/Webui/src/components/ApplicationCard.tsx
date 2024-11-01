import React from 'react';
import { Card, Button } from 'react-bootstrap';
import { Application } from '../types/types';

interface Props {
    application: Application;
    onSelect: (app: Application) => void;
    onDelete: (appId: number) => void;
}

const ApplicationCard: React.FC<Props> = ({ application, onSelect, onDelete }) => {

    

    return (
        <Card className="mb-3">
            <Card.Body>
                <Card.Title>{application.name}</Card.Title>
                <Card.Text>{application.description}</Card.Text>

                <>
                    <Button variant="primary" onClick={() => onSelect(application)}>
                        Посмотреть версии
                    </Button>
                    <Button
                        variant="outline-danger"
                        size="sm"
                        className="ms-3"
                        onClick={(e) => {
                            e.stopPropagation(); // Останавливаем всплытие события
                            onDelete(application.id);
                        }}
                    >
                        <i className="bi bi-trash"></i>
                    </Button>
                </>

            </Card.Body>
        </Card>
    );
};

export default ApplicationCard;