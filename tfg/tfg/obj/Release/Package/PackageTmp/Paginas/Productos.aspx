<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Productos.aspx.cs" Inherits="tfg.Paginas.Productos" %>

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
    <title>Productos</title>
    <link rel="shortcut icon" href="../imagenes/iconoPestaña.png" />


    <link rel="stylesheet" href="../default.css" />
    <link rel="stylesheet" href="../Estilos/Footer.css" />

    <script src="../default.js"></script>
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



    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script>
        function mostrarDetalleProducto(nombre) {
            // Codificar los parámetros de la URL para pasarlos a la página de detalle
            var encodedNombre = encodeURIComponent(nombre);


            // Construir la URL de la página de detalle del producto con los parámetros codificados
            var url = "DetalleProducto.aspx?nombre=" + encodedNombre;

            // Redirigir a la página de detalle del producto
            window.location.href = url;
        }
    </script>
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
        <div>


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

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div id="subcabecera" runat="server">
                    </div>

                    <div id="contenedor">
                        <div id="menuVerticalArticulos">
                            <!-- Contenido del div de menú vertical -->
                            <h2>ARTICULOS DE VENTA</h2>
                            <ol>
                                <li>
                                    <asp:LinkButton ID="LinkButton1" runat="server" OnClick='TipoProductoPanes_Click'>Panadería</asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="LinkButton2" runat="server" OnClick='TipoProductoBollos_Click'>Bollería</asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="LinkButton3" runat="server" OnClick='TipoProductoPasteles_Click'>Salados</asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="LinkButton4" runat="server" OnClick='TipoProductoTartas_Click'>Tartas</asp:LinkButton>
                                </li>
                            </ol>
                        </div>
                        <div id="filtrosProductos" runat="server">
                            <h3>Ordenar:</h3>
                            <asp:LinkButton ID="LinkButtonNombre" runat="server" OnClick='OrdenarPorNombre_Click' CssClass="boton-filtro">Nombre</asp:LinkButton>
                            <asp:LinkButton ID="LinkButtonPrecioBajo" runat="server" OnClick='OrdenarPorPrecioBajo_Click' CssClass="boton-filtro">Precio más bajo</asp:LinkButton>
                            <asp:LinkButton ID="LinkButtonPrecioAlto" runat="server" OnClick='OrdenarPorPrecioAlto_Click' CssClass="boton-filtro">Precio más alto</asp:LinkButton>
                            <asp:LinkButton ID="LinkButtonMejorValorado" runat="server" OnClick='OrdenarPorMejorValorado_Click' CssClass="boton-filtro">Mejor valorado</asp:LinkButton>
                        </div>


                        <div id="productosDisponibles" runat="server" class="productos-container"></div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>



                <div id="subfooter" class="subfooter">
    <p>Visítanos y disfruta de nuestros croissants recién horneados y tartas caseras.</p>
    <p>Te esperamos con una gran variedad de productos frescos y deliciosos.</p>
    <a href="/Paginas/Contacto.aspx" class="subfooter-link">Contáctanos para más información</a>
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
        <div><h6 style="text-align: center;">COPYRIGHT 2024 - TODOS LOS DERECHOS RESERVADOS.<br/>DISEÑO POR <a href="https://www.linkedin.com/in/ivan-almendros-lozano/" style="font-family: Oswalsd; font-size: 10px;">IVÁN ALMENDROS</a>
</h6>        </div>
    </form>
</body>
</html>
