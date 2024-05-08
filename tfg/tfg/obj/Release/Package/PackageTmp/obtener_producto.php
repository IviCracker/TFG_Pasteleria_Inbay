<?php
// Conexi�n a la base de datos
$conexion = new mysqli("localhost", "root", "", "tfg");

// Verificar conexi�n
if ($conexion->connect_error) {
    die("Conexi�n fallida: " . $conexion->connect_error);
}

// Obtener el nombre del producto desde la solicitud GET
$nombreProducto = $_GET["nombre"];

// Consulta para obtener la descripci�n, precio y stock del producto
$query = "SELECT descripcion, precio, stock FROM producto WHERE nombre = '" . $conexion->real_escape_string($nombreProducto) . "'";
$resultado = $conexion->query($query);

// Verificar si se encontr� el producto
if ($resultado->num_rows > 0) {
    // Obtener la informaci�n del producto
    $fila = $resultado->fetch_assoc();
    $producto = array(
        "descripcion" => $fila["descripcion"],
        "precio" => $fila["precio"],
        "stock" => $fila["stock"]
    );

    // Devolver la informaci�n del producto en formato JSON
    echo json_encode($producto);
} else {
    echo "Producto no encontrado";
}

// Cerrar conexi�n
$conexion->close();
?>
