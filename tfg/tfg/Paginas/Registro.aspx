<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registro.aspx.cs" Inherits="tfg.Paginas.Registro" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Registro</title>

    <link href="https://fonts.googleapis.com/css2?family=Oswald&display=swap" rel="stylesheet" />
    <link rel="stylesheet" href="../index.css" />
    <link rel="stylesheet" href="../Estilos/Footer.css" />
    <script src="https://kit.fontawesome.com/24693b33fa.js" crossorigin="anonymous"></script>

    <script src="../index.js"></script>

    <script type="module" src="https://unpkg.com/ionicons@7.1.0/dist/ionicons/ionicons.esm.js"></script>
    <script src="https://unpkg.com/ionicons@7.1.0/dist/ionicons/ionicons.js"></script>



</head>
<body>
    <form id="form1" runat="server">

        <div class="usuario">
            <ul>
                <% if (Session["UsuarioActual"] != null)
                    { %>
                <li><a href="Registro.aspx">
                    <ion-icon name="person"></ion-icon>
                    &nbsp;<%= Session["UsuarioActual"] %></a></li>
                <% }
                    else
                    { %>
                <li><a href="Registro.aspx">
                    <ion-icon name="lock-open"></ion-icon>
                    &nbsp;Registro</a></li>
                <% } %>
                <li><a href="MiCuenta.aspx">
                    <ion-icon name="person"></ion-icon>
                    &nbsp;Mi cuenta</a></li>
            </ul>
        </div>

        <header>
            <div class="logo">
                <a href="../default.aspx">
                    <img src="../imagenes/logoInbay4.png" width="150" /></a>
            </div>
            <input type="checkbox" id="menu-bar" />
            <label for="menu-bar">Menu</label>
            <nav class="navbar">
                <ul>
                    <li><a href="../default.aspx">Inicio</a></li>
                    <li><a href="Productos.aspx">Productos</a>
                        
                    </li>
                    <li><a href="Nosotros.aspx">Nosotros</a></li>
                    <li><a href="Contacto.aspx">Contacto</a>
                        
                    </li>
                    
                </ul>
            </nav>

            <div class="search-cart-container">
                <div class="search-container">
                    <input type="text" placeholder="Buscar..." class="search-input" />
                    <button type="submit" class="search-button">Buscar</button>
                </div>

            </div>
            <div class="cart-icon">
                <ion-icon name="cart"></ion-icon>
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
            <h2 class="tituloRegistroInicio">INICIAR SESIÓN</h2>
            <p>
                ¿No tienes cuenta aún?
        <asp:LinkButton ID="lnkMostrarRegistro" runat="server" Text=" ¡Crea una ya!" OnClick="btnMostrarRegistro_Click" CssClass="enlace-texto" />
            </p>
            <asp:Label ID="lblErrorInicioSesion" runat="server" CssClass="error-message" Visible="false"></asp:Label>

            <br />

            <div class="input-container">
                <i class="fa-solid fa-user"></i>
                <asp:TextBox ID="txtNombreUsuario" runat="server" placeholder="Nombre de usuario o correo electronico" CssClass="textbox" />
            </div>
            <div class="input-container">
                <i class="fa-solid fa-lock"></i>
                <asp:TextBox ID="txtContraseñaInicioSesion" runat="server" TextMode="Password" placeholder="Contraseña" CssClass="textbox" />
            </div>
            <asp:Button ID="btnIniciarSesion" runat="server" Text="Iniciar Sesión" OnClick="btnIniciarSesion_Click" CssClass="botonIniciarSesion" />
            <!-- Nuevo Label para mostrar mensajes de error -->
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
                <asp:Label ID="lblErrorNombre" runat="server" ForeColor="Red" Visible="false"></asp:Label>
            </div>
            <div class="input-container">
                <i class="fa-solid fa-envelope"></i>
                <asp:TextBox ID="txtCorreo" runat="server" TextMode="Email" placeholder="Correo electrónico" CssClass="textbox" />
                <asp:Label ID="lblErrorCorreo" runat="server" ForeColor="Red" Visible="false"></asp:Label>
            </div>
            <div class="input-container">
                <i class="fa-solid fa-phone"></i>
                <asp:TextBox ID="txtTelefono" runat="server" placeholder="Teléfono" CssClass="textbox" />
                <asp:Label ID="lblErrorTelefono" runat="server" ForeColor="Red" Visible="false"></asp:Label>
            </div>
            <div class="input-container">
                <i class="fa-solid fa-location-dot"></i>
                <asp:TextBox ID="txtDireccion" runat="server" placeholder="Dirección" CssClass="textbox" />
                <asp:Label ID="lblErrorDireccion" runat="server" ForeColor="Red" Visible="false"></asp:Label>
            </div>
            <div class="input-container">
                <i class="fa-solid fa-lock"></i>
                <asp:TextBox ID="txtContraseña" runat="server" TextMode="Password" placeholder="Contraseña" CssClass="textbox" />
                <asp:Label ID="lblErrorContraseña" runat="server" ForeColor="Red" Visible="false"></asp:Label>
            </div>
            <div class="input-container">
                <i class="fa-solid fa-unlock"></i>
                <asp:TextBox ID="txtRepetContraseña" runat="server" TextMode="Password" placeholder="Repetir contraseña" CssClass="textbox" />
                <asp:Label ID="lblErrorRepetContraseña" runat="server" ForeColor="Red" Visible="false"></asp:Label>
            </div>

            <div style="display: inline;">

                <asp:Button ID="btnRegistrarse" runat="server" Text="Registrarse" OnClick="btnRegistrarse_Click" />

            </div>
        </div>

        <div id="subfooter" class="subfooter">
            <p>¡Ven a nuestra pastelería y descubre un mundo de sabores exquisitos! Desde deliciosos croissants recién horneados hasta irresistibles tartas caseras, ¡te esperamos con los brazos abiertos!</p>
            <p>No te pierdas nuestras ofertas especiales de la semana y eventos exclusivos. ¡Haz tu día más dulce con nosotros!</p>
            <a href="Contacto.aspx" class="subfooter-link">¡Contáctanos para más información!</a>
        </div>

        <footer>
    <div class="container-footer w-container">
        <div class="w-row">
            <div class="footer-column w-clearfix w-col w-col-4">

                <img src="../imagenes/logoInbay4Vacio.png" alt="" width="200" class="failory-logo-image">
            </div>
            <div class="footer-column w-col w-col-8">
                <div class="w-row">
                    <div class="w-col w-col-8">
                        <div class="w-row">
                            <div class="w-col w-col-7 w-col-small-6 w-col-tiny-7">
                                <h3 class="footer-titles">Informacion</h3>
                                <p class="footer-links">
                                    <a href="../default.aspx"><span class="footer-link">Inicio<br/></span></a>
                                    <a href="Productos.aspx"><span class="footer-link">Productos<br/></span></a>
                                    <a href="Nosotros.aspx"><span class="footer-link">Nosotros<br/></span></a>
                                    <a href="Contacto.aspx"><span class="footer-link">Contacto<br/></span></a>
                                    <a href="HacerPedido.aspx"><span class="footer-link">Hacer Pedido</span></a><strong><br />
                                    </strong>
                                </p>
                            </div>
                            <div class="w-col w-col-5 w-col-small-6 w-col-tiny-5">
                                <h3 class="footer-titles">Otros</h3>
                                <p class="footer-links">
                                    <a href="https://g.co/kgs/eR36i2S"><span class="footer-link">Pasteleria Inbay<br/></span></a>
                                    <a href="tel:915722017"><span class="footer-link">915 72 20 17<br/></span></a>
                                    <a href="https://maps.app.goo.gl/GMDvFgaLnnw97igS6"><span class="footer-link" style="white-space: nowrap; display: block;">Calle Capitan Blanco<br/> Argibay 43<br/></span></a>
                                    <strong><br/>
                                    </strong>
                                </p>
                            </div>
                        </div>
                    </div>
                    <div class="column-center-mobile w-col w-col-4">
                        <h3 class="footer-titles">Siguenos en nuestras redes sociales!</h3>
                        <a href="https://www.instagram.com/pasteleriainbay/" target="_blank" class="footer-social-network-icons w-inline-block">
                                <img class="imagenesRedesSociales" src="https://uploads-ssl.webflow.com/5966ea9a9217ca534caf139f/5c8dbfe70fcf5a0514c5b1da_Instagram%20Icon.svg" width="20" alt="Instagram icon"/></a><a href="https://www.facebook.com/pasteleriainbay/?locale=es_ES" target="_blank" class="footer-social-network-icons w-inline-block">
                                    <img class="imagenesRedesSociales" src="https://uploads-ssl.webflow.com/5966ea9a9217ca534caf139f/5c8dbe42e1e6034fdaba46f6_Facebook%20Icon.svg" width="20" alt="Facebook Icon"/></a>
                        <p class="footer-description">
                            Enviame un correo a: <a href="mailto:pasteleriainbay@gmail.com"><strong class="link-email-footer">PasteleriaInbay@gmail.com</strong></a><br />
                        </p>
                    </div>
                </div>
            </div>
        </div>
    </div>
</footer>
<div>
    <h6>COPYRIGHT 2024 - TODOS LOS DERECHOS RESERVADOS. DISEÑO POR <a href="https://www.linkedin.com/in/ivan-almendros-lozano/">IVÁN ALMENDROS</a></h6>
</div>

    </form>
</body>
</html>
