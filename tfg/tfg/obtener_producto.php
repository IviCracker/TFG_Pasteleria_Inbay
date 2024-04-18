<?php
// Conexión a la base de datos
$conexion = new mysqli("localhost", "root", "", "tfg");

// Verificar conexión
if ($conexion->connect_error) {
    die("Conexión fallida: " . $conexion->connect_error);
}

// Obtener el nombre del producto desde la solicitud GET
$nombreProducto = $_GET["nombre"];

// Consulta para obtener la descripción, precio y stock del producto
$query = "SELECT descripcion, precio, stock FROM producto WHERE nombre = '" . $conexion->real_escape_string($nombreProducto) . "'";
$resultado = $conexion->query($query);

// Verificar si se encontró el producto
if ($resultado->num_rows > 0) {
    // Obtener la información del producto
    $fila = $resultado->fetch_assoc();
    $producto = array(
        "descripcion" => $fila["descripcion"],
        "precio" => $fila["precio"],
        "stock" => $fila["stock"]
    );

    // Devolver la información del producto en formato JSON
    echo json_encode($producto);
} else {
    echo "Producto no encontrado";
}

// Cerrar conexión
$conexion->close();
?>
