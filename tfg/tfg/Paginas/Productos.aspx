<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Productos.aspx.cs" Inherits="tfg.Paginas.Productos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Productos</title>

    <link href="https://fonts.googleapis.com/css2?family=Oswald&display=swap" rel="stylesheet" />
    <link rel="stylesheet" href="../index.css" />
    <script src="../index.js"></script>

    <link rel="stylesheet" href="../fonts/icomoon/style.css" />
    <link rel="stylesheet" href="../css/owl.carousel.min.css" />
    <link rel="stylesheet" href="../css/style.css" />

    <script type="module" src="https://unpkg.com/ionicons@7.1.0/dist/ionicons/ionicons.esm.js"></script>
    <script src="https://unpkg.com/ionicons@7.1.0/dist/ionicons/ionicons.js"></script>
    <script src="../js/jquery-3.3.1.min.js"></script>
    <script src="../js/popper.min.js"></script>
    <script src="../js/owl.carousel.min.js"></script>
    <script src="../js/main.js"></script>

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


            <header>
                <div class="logo">
                    <a href="../index.aspx">
                        <img src="../imagenes/logoInbay3.png" width="150" /></a>
                </div>
                <input type="checkbox" id="menu-bar" />
                <label for="menu-bar">Menu</label>
                <nav class="navbar">
                    <ul>
                        <li><a href="../index.aspx">Inicio</a></li>
                        <li><a href="../Paginas/Productos.aspx">Productos</a>
                            <ul class="submenu" id="productos">
                                <li><a href="#">Panes</a></li>
                                <li><a href="#">Bollería</a></li>
                                <li><a href="#">Pastelería</a></li>
                                <li><a href="#">Tartas</a></li>
                            </ul>
                        </li>
                        <li><a href="#">Nosotros</a></li>
                        <li><a href="#">Contacto</a>
                            <ul class="submenu" id="contacto">
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

                </div>
                <div class="cart-icon">
                    <ion-icon size="large" name="cart-outline"></ion-icon>
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
            <div id="subcabecera" runat="server">
            </div>

            <div id="contenedor">
                <div id="menuVerticalArticulos">
                    <!-- Contenido del div de menú vertical -->
                    <h2>ARTICULOS DE VENTA</h2>
                    <ol>
                        <li>
                            <asp:LinkButton ID="LinkButton1" runat="server" OnClick='TipoProductoPanes_Click'>Panes</asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="LinkButton2" runat="server" OnClick='TipoProductoBollos_Click'>Bollos</asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="LinkButton3" runat="server" OnClick='TipoProductoPasteles_Click'>Pasteles</asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="LinkButton4" runat="server" OnClick='TipoProductoTartas_Click'>Tartas</asp:LinkButton>
                        </li>
                    </ol>
                </div>
                <div id="filtrosProductos" runat="server">
                    <asp:LinkButton ID="LinkButtonNombre" runat="server" OnClick='OrdenarPorNombre_Click' CssClass="boton-filtro">Nombre</asp:LinkButton>
                    <asp:LinkButton ID="LinkButtonPrecioBajo" runat="server" OnClick='OrdenarPorPrecioBajo_Click' CssClass="boton-filtro">Precio más bajo</asp:LinkButton>
                    <asp:LinkButton ID="LinkButtonPrecioAlto" runat="server" OnClick='OrdenarPorPrecioAlto_Click' CssClass="boton-filtro">Precio más alto</asp:LinkButton>
                    <asp:LinkButton ID="LinkButtonMejorValorado" runat="server" OnClick='OrdenarPorMejorValorado_Click' CssClass="boton-filtro">Mejor valorado</asp:LinkButton>
                </div>

                <div id="productosDisponibles" runat="server" class="productos-container">
                    <!-- Contenido del div de productos disponibles -->
                </div>
            </div>


        </div>
    </form>
</body>
</html>
