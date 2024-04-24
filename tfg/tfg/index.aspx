<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="tfg.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Pasteleria Inbay TFG</title>

    <link href="https://fonts.googleapis.com/css2?family=Oswald&display=swap" rel="stylesheet" />
    <link rel="stylesheet" href="index.css" />
    <link rel="stylesheet" href="/Estilos/Footer.css" />

    <script src="index.js"></script>

    <link rel="stylesheet" href="fonts/icomoon/style.css" />
    <link rel="stylesheet" href="css/owl.carousel.min.css" />
    <link rel="stylesheet" href="css/style.css" />
</head>



<body>
    <form id="form1" runat="server">
        <div class="usuario">
            <ul>
                <li><a href="/Paginas/Registro.aspx">
                    <ion-icon name="lock-open"></ion-icon>
                    &nbsp;Registro</a></li>
                <li><a href="/Paginas/MiCuenta.aspx">
                    <ion-icon name="person"></ion-icon>
                    &nbsp;Mi cuenta</a></li>
            </ul>
        </div>
        <div>
            <main>

                <header>
                    <div class="logo">
                        <a href="index.aspx">
                            <img src="imagenes/logoInbay3.png" width="150" /></a>
                    </div>
                    <input type="checkbox" id="menu-bar" />
                    <label for="menu-bar">Menu</label>
                    <nav class="navbar">
                        <ul>
                            <li><a href="index.aspx">Inicio</a></li>
                            <li><a href="Paginas/Productos.aspx">Productos</a>
                                <ul class="submenu" id="productos">
                                    <li><a href="#">Panes</a></li>
                                    <li><a href="#">Bollería</a></li>
                                    <li><a href="#">Pastelería</a></li>
                                    <li><a href="#">Tartas</a></li>
                                </ul>
                            </li>
                            <li><a href="#">Nosotros</a></li>
                            <li><a href="#" onclick="mostrarSubMenu('contacto')">Contacto</a>
                                <ul class="submenu" id="contacto">
                                    <li><a href="#">Dónde estamos</a></li>
                                </ul>
                            </li>
                            <li><a href="#" onclick="cambiarVista(4)">Hacer Pedido</a></li>
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

                <div class="content">

                    <div class="container">

                        <div class="owl-carousel owl-1">
                            <div>
                                <img src="imagenes/tarjeta inbay1.jpg" alt="Image" class="img-fluid" />
                            </div>
                            <div>
                                <img src="images/hero_2.jpg" alt="Image" class="img-fluid" />
                            </div>
                            <div>
                                <img src="images/hero_3.jpg" alt="Image" class="img-fluid" />
                            </div>
                            <div>
                                <img src="images/hero_3.jpg" alt="Image" class="img-fluid" />
                            </div>
                        </div>
                    </div>
                </div>
                <br />
                <br />

                <div class="productos-destacados">
                    <h2 style="text-align: center; font-size: 26px;">Productos Destacados</h2>
                    <div id="productosDestacadosContainer" runat="server" class="productos-container">
                        <!-- Aquí se cargarán dinámicamente los productos desde el servidor -->
                    </div>
                </div>

                <!-- Modales para los productos -->
                <!-- Modal -->
                <div id="modal" class="modal">
                    <div class="modal-content">
                        <span class="close" onclick="closeModal()">&times;</span>
                        <div class="product-info">
                            <img id="modalImage" src="#" alt="Producto" class="product-image" />
                            <div class="product-details">
                                <h2 id="modalProductName" class="nombre-producto" runat="server">Nombre del Producto</h2>
                                <p id="modalDescription" runat="server">Descripción del producto aquí...</p>
                                <p id="modalPrice" runat="server">Precio: $XX.XX</p>
                                <p id="modalStock" runat="server">Stock: XX</p>
                                <button>Agregar al carrito</button>
                            </div>
                        </div>
                    </div>
                </div>





                <div class="ubicacion-horario">
                    <h2>Horario y Ubicación</h2>
                    <p><strong>Horario de Apertura:</strong></p>
                    <ul>
                        <li>Lunes - Viernes: 8:30 AM - 2:30 PM, 5:00 PM - 8:00 PM</li>
                        <li>Sábado - Domingo: 8:30 AM - 2:30 PM</li>
                    </ul>
                </div>



                <div class="elfsight-app-1224c210-7a6c-478b-a82a-764cf5fa5110" data-elfsight-app-lazy>&nbsp</div>


            </main>
        </div>
        <footer>
            <div class="container-footer w-container">
                <div class="w-row">
                    <div class="footer-column w-clearfix w-col w-col-4">
                        <img src="https://uploads-ssl.webflow.com/5966ea9a9217ca534caf139f/596d33f36607b12cfdaf8ad2_LogoWhite.png" alt="" width="40" class="failory-logo-image">
                        <h3 class="footer-failory-name">Failory</h3>
                        <p class="footer-description-failory">Lorem ipsum dolor sit amet.<br>
                        </p>
                    </div>
                    <div class="footer-column w-col w-col-8">
                        <div class="w-row">
                            <div class="w-col w-col-8">
                                <div class="w-row">
                                    <div class="w-col w-col-7 w-col-small-6 w-col-tiny-7">
                                        <h3 class="footer-titles">Learn</h3>
                                        <p class="footer-links"><a href="" target="_blank"><span class="footer-link">Failed Startups<br>
                                        </span></a><a href=""><span class="footer-link">Successful Startups<br>
                                        </span></a><a href=""><span class="footer-link">Blog</span></a><span><br>
                                        </span><a href=""><span class="footer-link">Entrepreneurial Tools<br>
                                        </span></a><a href=""><span class="footer-link">Startup Cemetery<br>
                                        </span></a><a href=""><span class="footer-link">Podcast</span></a><strong><br>
                                        </strong></p>
                                    </div>
                                    <div class="w-col w-col-5 w-col-small-6 w-col-tiny-5">
                                        <h3 class="footer-titles">Other</h3>
                                        <p class="footer-links"><a href=""><span class="footer-link">Sponsor Us!<br>
                                        </span></a><a href=""><span class="footer-link">Open Startup<br>
                                        </span></a><a href=""><span class="footer-link"></span></a><a href=""><span class="footer-link">Contribute<br>
                                        </span></a><a href=""><span class="footer-link">FAQ</span></a><strong><br>
                                        </strong></p>
                                    </div>
                                </div>
                            </div>
                            <div class="column-center-mobile w-col w-col-4">
                                <h3 class="footer-titles">Follow Us!</h3>
                                <a href="" target="_blank" class="footer-social-network-icons w-inline-block">
                                    <img src="https://uploads-ssl.webflow.com/5966ea9a9217ca534caf139f/5c8dbf0a2f2b67e3b3ba079c_Twitter%20Icon.svg" width="20" alt="Twitter icon"></a><a href="" target="_blank" class="footer-social-network-icons w-inline-block"><img src="https://uploads-ssl.webflow.com/5966ea9a9217ca534caf139f/5c8dbfe70fcf5a0514c5b1da_Instagram%20Icon.svg" width="20" alt="Instagram icon"></a><a href="" target="_blank" class="footer-social-network-icons w-inline-block"><img src="https://uploads-ssl.webflow.com/5966ea9a9217ca534caf139f/5c8dbe42e1e6034fdaba46f6_Facebook%20Icon.svg" width="20" alt="Facebook Icon"></a><a href="" target="_blank" class="footer-social-network-icons w-inline-block"><img src="https://uploads-ssl.webflow.com/5966ea9a9217ca534caf139f/5c8dc0002f2b676eb4ba0869_LinkedIn%20Icon.svg" width="20" alt="LinkedIn Icon"></a><a href="" target="_blank" class="footer-social-network-icons w-inline-block"><img src="https://uploads-ssl.webflow.com/5966ea9a9217ca534caf139f/5c8dc0112f2b6739c9ba0871_Pinterest%20Icon.svg" width="20" alt="Pinterest Icon" class=""></a>
                                <p class="footer-description">Email me at: <a href=""><strong class="link-email-footer">Lorem Ipsum</strong></a><br>
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </footer>
    </form>
    <script src="https://static.elfsight.com/platform/platform.js" data-use-service-core defer></script>
    <script src='https://widgets.sociablekit.com/google-reviews/widget.js' async defer></script>
    <script type="module" src="https://unpkg.com/ionicons@7.1.0/dist/ionicons/ionicons.esm.js"></script>
    <script src="https://unpkg.com/ionicons@7.1.0/dist/ionicons/ionicons.js"></script>
    <script src="js/jquery-3.3.1.min.js"></script>
    <script src="js/popper.min.js"></script>
    <script src="js/owl.carousel.min.js"></script>
    <script src="js/main.js"></script>
    <script>

</script>
</body>
</html>


