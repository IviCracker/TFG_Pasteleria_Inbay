<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DatosTarjeta.aspx.cs" Inherits="tfg.Paginas.DatosTarjeta" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <!-- Google tag (gtag.js) -->
    <script async src="https://www.googletagmanager.com/gtag/js?id=G-JBGTE8PV6Y"></script>
    <script>
        window.dataLayer = window.dataLayer || [];
        function gtag() { dataLayer.push(arguments); }
        gtag('js', new Date());

        gtag('config', 'G-JBGTE8PV6Y');
    </script>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <title>Datos de la tarjeta</title>

    <link rel="stylesheet" href="../default.css" />
    <link rel="stylesheet" href="../Estilos/Footer.css" />
    <link rel="stylesheet" href="../Estilos/DatosTarjeta.css" />


    <script src="https://kit.fontawesome.com/24693b33fa.js" crossorigin="anonymous"></script>

    <script src="../default.js"></script>

    <script type="module" src="https://unpkg.com/ionicons@7.1.0/dist/ionicons/ionicons.esm.js"></script>
    <script src="https://unpkg.com/ionicons@7.1.0/dist/ionicons/ionicons.js"></script>
    <script>
        document.querySelectorAll('.imagen-producto').forEach(function (producto) {
            producto.addEventListener('click', function () {
                var enlace = this.querySelector('a');
                if (enlace) {
                    window.location.href = enlace.getAttribute('data-url');
                }
            });
        });
    </script>
    <script type="text/javascript">
        function mostrarModalSinTarjetas() {
            // Mostrar el modal
            document.getElementById('modalSinTarjetas').style.display = 'block';
        }

        function cerrarModal() {
            // Ocultar el modal
            document.getElementById('modalSinTarjetas').style.display = 'none';
        }
    </script>
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
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="search-input" Placeholder="Buscar..." />
                    <asp:Button ID="searchButton" runat="server" Text="Buscar" CssClass="search-button" OnClick="searchButton_Click" />
                </div>

            </div>
            <div class="cart-icon" id="cartIcon">
                <ion-icon name="cart"></ion-icon>
            </div>
            <div class="cart-panel" id="cartPanel">
                <button type="button" class="close-btn" id="closeCartPanel">&times;</button>

                <h2>Carrito de compra</h2>
                <% if (Session["UsuarioActual"] == null)
                    { %>
                <p>Inicia sesión antes de ver tu carrito</p>
                <% } %>

                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <div id="productosCarritoContainer" runat="server" class="productos-carrito"></div>
                    </ContentTemplate>

                </asp:UpdatePanel>



                <div id="cartInfoContainer" runat="server">
                </div>

                <% 
                    if (Session["UsuarioActual"] != null)
                    {
                        int idCliente = ObtenerIdCliente(); // Supongamos que tienes una función para obtener el ID del cliente

                        if (comprobarCarrito(idCliente) > 0)
                        {
                %>
                <asp:Button ID="verCarritoBtn" runat="server" Text="Realizar pago" OnClick="VerCarritoBtn_Click" CssClass="ver-carrito-btn" />
                <% 
                        }
                    }
                %>
            </div>


        </header>


        <div id="modalSinTarjetas" class="modal">
            <div class="modal-content">
                <span class="close" onclick="cerrarModal()">&times;</span>
                <p>No tienes ninguna tarjeta añadida. Por favor, añade una tarjeta.</p>
            </div>
        </div>
        <%-- Datos Tarjeta --%>

        <div class="tarjetaCredito">
            <div class="datosTarjeta">
                <div class="grupo">
                    <span for="numeroTarjeta">Número de Tarjeta:</span>
                    <asp:TextBox ID="txtNumeroTarjeta" runat="server" placeholder="Número de Tarjeta" MaxLength="16"></asp:TextBox>
                </div>
                <div class="grupo">
                    <span for="nombreTarjeta">Nombre en la Tarjeta:</span>
                    <asp:TextBox ID="txtNombreTarjeta" runat="server" placeholder="Nombre en la Tarjeta"></asp:TextBox>
                </div>
                <div class="grupo">
                    <span for="fechaExpiracion">Fecha de Expiración:</span>
                    <asp:DropDownList ID="mesExpiracion" runat="server">
                        <asp:ListItem Text="Mes" Value="" />
                        <asp:ListItem Text="Enero" Value="01" />
                        <asp:ListItem Text="Febrero" Value="02" />
                        <asp:ListItem Text="Marzo" Value="03" />
                        <asp:ListItem Text="Abril" Value="04" />
                        <asp:ListItem Text="Mayo" Value="05" />
                        <asp:ListItem Text="Junio" Value="06" />
                        <asp:ListItem Text="Julio" Value="07" />
                        <asp:ListItem Text="Agosto" Value="08" />
                        <asp:ListItem Text="Septiembre" Value="09" />
                        <asp:ListItem Text="Octubre" Value="10" />
                        <asp:ListItem Text="Noviembre" Value="11" />
                        <asp:ListItem Text="Diciembre" Value="12" />
                    </asp:DropDownList>
                    <asp:DropDownList ID="anioExpiracion" runat="server">
                        <asp:ListItem Text="Año" Value="" />
                        <asp:ListItem Text="2024" Value="01" />
                        <asp:ListItem Text="2025" Value="02" />
                        <asp:ListItem Text="2026" Value="03" />
                        <asp:ListItem Text="2027" Value="04" />
                        <asp:ListItem Text="2028" Value="05" />
                        <asp:ListItem Text="2029" Value="06" />
                        <asp:ListItem Text="2030" Value="07" />
                        <asp:ListItem Text="2031" Value="08" />
                        <asp:ListItem Text="2032" Value="09" />
                        <asp:ListItem Text="2033" Value="10" />
                        <asp:ListItem Text="2034" Value="11" />
                    </asp:DropDownList>
                </div>
                <div class="grupo">
                    <span for="cvv">CVV:</span>
                    <asp:TextBox ID="txtCVV" runat="server" placeholder="CVV" MaxLength="3"></asp:TextBox>
                </div>
                <asp:Button ID="btnGuardarDatosTarjeta" runat="server" Text="Guardar Datos" OnClick="BtnGuardarDatosTarjeta_Click" CssClass="btnGuardar" />
                <asp:Button ID="btnBorrarDatosTarjeta" runat="server" Text="Borrar Datos" OnClick="BtnBorrarDatosTarjeta_Click" CssClass="btnGuardar" />
            </div>
        </div>













        <div id="subfooter" class="subfooter">
            <p>¡Ven a nuestra pastelería y descubre un mundo de sabores exquisitos! Desde deliciosos croissants recién horneados hasta irresistibles tartas caseras, ¡te esperamos con los brazos abiertos!</p>
            <p>No te pierdas nuestras ofertas especiales de la semana y eventos exclusivos. ¡Haz tu día más dulce con nosotros!</p>
            <a href="/Paginas/Contacto.aspx" class="subfooter-link">¡Contáctanos para más información!</a>
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
                                            <a href="../default.aspx"><span class="footer-link">Inicio<br />
                                            </span></a>
                                            <a href="Productos.aspx"><span class="footer-link">Productos<br />
                                            </span></a>
                                            <a href="Nosotros.aspx"><span class="footer-link">Nosotros<br />
                                            </span></a>
                                            <a href="Contacto.aspx"><span class="footer-link">Contacto<br />
                                            </span></a>
                                            <a href="HacerPedido.aspx"><span class="footer-link">Hacer Pedido</span></a><strong><br />
                                            </strong>
                                        </p>
                                    </div>
                                    <div class="w-col w-col-5 w-col-small-6 w-col-tiny-5">
                                        <h3 class="footer-titles">Otros</h3>
                                        <p class="footer-links">
                                            <a href="https://g.co/kgs/eR36i2S"><span class="footer-link">Pasteleria Inbay<br />
                                            </span></a>
                                            <a href="tel:915722017"><span class="footer-link">915 72 20 17<br />
                                            </span></a>
                                            <a href="https://maps.app.goo.gl/GMDvFgaLnnw97igS6"><span class="footer-link" style="white-space: nowrap; display: block;">Calle Capitan Blanco<br />
                                                Argibay 43<br />
                                            </span></a>
                                            <strong>
                                                <br />
                                            </strong>
                                        </p>
                                    </div>
                                </div>
                            </div>
                            <div class="column-center-mobile w-col w-col-4">
                                <h3 class="footer-titles">Siguenos en nuestras redes sociales!</h3>
                                <a href="https://www.instagram.com/pasteleriainbay/" target="_blank" class="footer-social-network-icons w-inline-block">
                                    <img class="imagenesRedesSociales" src="https://uploads-ssl.webflow.com/5966ea9a9217ca534caf139f/5c8dbfe70fcf5a0514c5b1da_Instagram%20Icon.svg" width="20" alt="Instagram icon" /></a><a href="https://www.facebook.com/pasteleriainbay/?locale=es_ES" target="_blank" class="footer-social-network-icons w-inline-block">
                                        <img class="imagenesRedesSociales" src="https://uploads-ssl.webflow.com/5966ea9a9217ca534caf139f/5c8dbe42e1e6034fdaba46f6_Facebook%20Icon.svg" width="20" alt="Facebook Icon" /></a>
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
