<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DetalleProducto.aspx.cs" Inherits="tfg.Paginas.DetalleProducto" %>

<!DOCTYPE html>
<html>
<head>
    <title>Detalles del Producto</title>
    <!-- Aquí puedes incluir tus estilos CSS -->
</head>
<body>
    <h1>Detalles del Producto</h1>
    
    <!-- Mostrar la imagen del producto -->
    <img src="<%= Request.QueryString["imagenUrl"] %>" alt="<%= Request.QueryString["nombre"] %>" style="width: 250px; height: 250px; object-fit: cover;">
    
    <!-- Mostrar el nombre del producto -->
    <h2><%= Request.QueryString["nombre"] %></h2>
    
    <!-- Mostrar el precio del producto -->
    <p>Precio: <%= Request.QueryString["precio"] %></p>
    
    <!-- Mostrar la valoración del producto -->
    <p>Valoración: <%= Request.QueryString["valoracion"] %> estrellas</p>
</body>
</html>
