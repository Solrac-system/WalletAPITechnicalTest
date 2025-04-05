# Wallet API

Wallet API es un servicio RESTful para gestionar billeteras digitales y sus transacciones. 
Permite realizar operaciones como la creación de billeteras, adición de créditos, débito de saldos, y consulta de transacciones históricas.

## Endpoints Principales

### **Billeteras**
- **POST /api/Wallet**: Crea una nueva billetera.
- **GET /api/Wallet/{id}**: Obtiene información de una billetera específica.
- **PUT /api/Wallet/{id}**: Actualiza información de una billetera existente.
- **DELETE /api/Wallet/{id}**: Elimina una billetera.

### **Transacciones**
- **POST /api/Transaction**: Crea una transacción asociada a una billetera. Las transacciones pueden ser:
  - **Credit**: Agrega saldo a la billetera.
  - **Debit**: Resta saldo de la billetera.
- **GET /api/Transaction/{walletId}**: Obtiene el historial de transacciones de una billetera.

  ## Detalles del Comportamiento

### **Transacciones: Credit y Debit**
Cuando se realiza una transacción, la lógica de la API ajusta automáticamente el saldo de la billetera asociada:

1. **Credit (Ingreso)**:
   - Una transacción de tipo `Credit` incrementa el saldo de la billetera.
   - Ejemplo:
     - Saldo Inicial: $500
     - Transacción: `Credit` $200
     - Saldo Final: $700

2. **Debit (Egreso)**:
   - Una transacción de tipo `Debit` reduce el saldo de la billetera.
   - Si el monto de la transacción excede el saldo actual, la API lanza un error de tipo `InvalidOperationException` con el mensaje `"Insufficient balance."`.
   - Ejemplo:
     - Saldo Inicial: $500
     - Transacción: `Debit` $300
     - Saldo Final: $200


## Ejemplo de Uso

### **Crear una Billetera**

curl -X POST http://localhost:5000/api/Wallet \
-H "Content-Type: application/json" \
-d '{
  "documentId": "123456789",
  "name": "Carlos Pérez",
  "balance": 500.00
}'

### **Crear una Transacción (Credit)**

curl -X POST http://localhost:5000/api/Transaction \
-H "Content-Type: application/json" \
-d '{
  "walletId": 1,
  "amount": 200.00,
  "type": "Credit"
}'
### **Crear una Transacción (Debit)**
curl -X POST http://localhost:5000/api/Transaction \
-H "Content-Type: application/json" \
-d '{
  "walletId": 1,
  "amount": 300.00,
  "type": "Debit"
}'

## Errores Comunes

### **Transacciones**
- **`400 Bad Request`**:
  - Ocurre si el monto es negativo o no se especifica un tipo válido (`Credit` o `Debit`).
  - Ejemplo de respuesta:
    ```json
    {
      "message": "Transaction amount must be greater than zero."
    }
    ```

- **`404 Not Found`**:
  - Ocurre si la billetera especificada no existe.
  - Ejemplo de respuesta:
    ```json
    {
      "message": "Wallet not found."
    }
    ```

- **`400 Bad Request` (Saldo Insuficiente)**:
  - Ocurre si se intenta realizar un débito mayor al saldo disponible.
  - Ejemplo de respuesta:
    ```json
    {
      "message": "Insufficient balance."
    }
    ```

## Configuración

### **Requisitos**
- .NET 8 o superior
- SQL Server LocalDB

## Instalación
1. Clonar el repositorio: `git clone https://github.com/Solrac-system/WalletAPITechnicalTest.git`
2. Configurar la conexión a la base de datos en el archivo `appsettings.json`.
3. Ejecutar las migraciones: `dotnet ef database update`.


