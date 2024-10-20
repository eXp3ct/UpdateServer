import React from 'react';
import { Modal, Button, Form } from 'react-bootstrap';

function VersionModal({ show, handleClose, appName }) {
  return (
    <Modal show={show} onHide={handleClose}>
      <Modal.Header closeButton>
        <Modal.Title>Создать версию для {appName}</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <Form>
          <Form.Group controlId="version">
            <Form.Label>Версия</Form.Label>
            <Form.Control type="text" placeholder="Введите версию (например, 1.0.0.0)" />
          </Form.Group>

          <Form.Group controlId="releaseDate" className="mt-3">
            <Form.Label>Дата выпуска</Form.Label>
            <Form.Control type="date" />
          </Form.Group>

          <Form.Group controlId="isMandatory" className="mt-3">
            <Form.Check type="checkbox" label="Обязательная версия" />
          </Form.Group>
        </Form>
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={handleClose}>
          Закрыть
        </Button>
        <Button variant="primary">Сохранить</Button>
      </Modal.Footer>
    </Modal>
  );
}

export default VersionModal;
