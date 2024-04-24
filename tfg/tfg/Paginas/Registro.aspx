<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registro.aspx.cs" Inherits="tfg.Paginas.Registro" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Registro</title>

    <link href="https://fonts.googleapis.com/css2?family=Oswald&display=swap" rel="stylesheet" />
    <link rel="stylesheet" href="../index.css" />
    <script src="https://kit.fontawesome.com/24693b33fa.js" crossorigin="anonymous"></script>

    <script src="../index.js"></script>

    <script type="module" src="https://unpkg.com/ionicons@7.1.0/dist/ionicons/ionicons.esm.js"></script>
    <script src="https://unpkg.com/ionicons@7.1.0/dist/ionicons/ionicons.js"></script>


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
        <br />

        <div id="textoIniciarSesion" runat="server" class="formulario">
            <h1 class="tituloRegistroInicio">INICIAR SESIÓN</h1>
            <p>
                ¿No tienes cuenta aún?
                <asp:LinkButton ID="lnkMostrarRegistro" runat="server" Text=" ¡Crea una ya!" OnClick="btnMostrarRegistro_Click" CssClass="enlace-texto" />
            </p>
            <br />
            <div class="input-container">
                <i class="fa-solid fa-user"></i>
                <asp:TextBox ID="txtNombreUsuario" runat="server" placeholder="Nombre de usuario" CssClass="textbox" />
            </div>
            <div class="input-container">
                <i class="fa-solid fa-lock"></i>
                <asp:TextBox ID="txtContraseñaInicioSesion" runat="server" TextMode="Password" placeholder="Contraseña" CssClass="textbox" />
            </div>
            <asp:Button ID="btnIniciarSesion" runat="server" Text="Iniciar Sesión" OnClick="btnIniciarSesion_Click" />
        </div>

        <div id="textoCrearCuenta" runat="server" class="formulario" style="display: none;">
            <!-- Ocultar el formulario de registro por defecto -->

            <h1 class="tituloRegistroInicio">CREAR CUENTA</h1>
            <p>
                ¿Ya tiene una cuenta?
    <asp:LinkButton ID="lnkIniciarSesion" runat="server" Text="¡Inicie sesión!" OnClick="lnkIniciarSesion_Click" CssClass="enlace-texto" />
            </p>

            <div class="input-container">
                <i class="fa-solid fa-user"></i>
                <asp:TextBox ID="txtNombre" runat="server" placeholder="Nombre" CssClass="textbox" />
            </div>
            <div class="input-container">
                <i class="fa-solid fa-envelope"></i>
                <asp:TextBox ID="txtCorreo" runat="server" TextMode="Email" placeholder="Correo electrónico" CssClass="textbox" />
            </div>
            <div class="input-container">
                <i class="fa-solid fa-phone"></i>
                <asp:TextBox ID="txtTelefono" runat="server" placeholder="Teléfono" CssClass="textbox" />
            </div>
            <div class="input-container">
                <i class="fa-solid fa-location-dot"></i>
                <asp:TextBox ID="txtDireccion" runat="server" placeholder="Dirección" CssClass="textbox" />
            </div>
            <div class="input-container">
                <i class="fa-solid fa-lock"></i>
                <asp:TextBox ID="txtContraseña" runat="server" TextMode="Password" placeholder="Contraseña" CssClass="textbox" />
            </div>
            <div class="input-container">
                <i class="fa-solid fa-unlock"></i>
                <asp:TextBox ID="txtRepetContraseña" runat="server" TextMode="Password" placeholder="Repetir contraseña" CssClass="textbox" />
            </div>
            <div style="display: inline;">

                <asp:Button ID="btnRegistrarse" runat="server" Text="Registrarse" OnClick="btnRegistrarse_Click" />

            </div>
        </div>



    </form>
</body>
</html>
