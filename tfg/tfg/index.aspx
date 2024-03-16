<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="tfg.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Pasteleria Inbay TFG</title>

    <link href="https://fonts.googleapis.com/css2?family=Oswald&display=swap" rel="stylesheet" />
    <link rel="stylesheet" href="index.css" />
    <script src="index.js"></script>
</head>



<body>
    <form id="form1" runat="server">
        <div class="usuario">
            <ul>
                <li><a href="#">
                    <ion-icon name="lock-open"></ion-icon>
                    &nbsp;Registro</a></li>
                <li><a href="#">
                    <ion-icon name="person"></ion-icon>
                    &nbsp;Mi cuenta</a></li>
            </ul>
        </div>
        <div>
            <main>
                <header>
                    <div class="logo">
                        <a href="index.aspx">
                            <img src="imagenes/logoInbay1.png" width="150" /></a>
                    </div>
                    <nav>
                        <ul>
                            <li><a href="#">Inicio</a></li>
                            <li><a href="#">Productos</a>
                                <ul class="submenu">
                                    <li><a href="#">Panes</a></li>
                                    <li><a href="#">Bollería</a></li>
                                    <li><a href="#">Pastelería</a></li>
                                    <li><a href="#">Tartas</a></li>
                                </ul>
                            </li>

                            <li><a href="#">Nosotros</a></li>
                            <li><a href="#">Contacto</a>
                                <ul class="submenu">
                                    <li><a href="#">Dónde estamos</a></li>
                                </ul>
                            </li>
                            <li><a href="#">Hacer Pedido</a></li>
                        </ul>
                    </nav>
                    <div class="search-cart-container">
                        <div class="search-container">
                            <input type="text" placeholder="Buscar..." class="search-input" />
                            <button type="submit" class="search-button">Buscar</button>
                        </div>
                        <div class="cart-icon">
                            <ion-icon size="large" name="cart-outline"></ion-icon>
                        </div>
                    </div>
                    <div class="cart-panel">
                        <h2>Carrito de compra</h2>
                        <ul>
                            <li>Producto 1</li>
                            <li>Producto 2</li>
                            <li>Producto 3</li>
                        </ul>   
                    </div>

                </header>

                <p></p>


            </main>
        </div>
    </form>
    <script type="module" src="https://unpkg.com/ionicons@7.1.0/dist/ionicons/ionicons.esm.js"></script>
    <script nomodule src="https://unpkg.com/ionicons@7.1.0/dist/ionicons/ionicons.js"></script>
</body>
</html>


