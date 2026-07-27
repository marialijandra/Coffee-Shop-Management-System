<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <section class="hero">
        <div class="container">
            <div class="hero-eyebrow">
                <span class="eyebrow-left">Coffee crafted for curious palates</span>
                <span class="special">Today's Special &nbsp;&bull;&nbsp; 15% Off Today</span>
            </div>

            <h1 class="hero-watermark">
                <span class="wm-row">CINNAMON CREAM LATTE</span>
                <span class="wm-row">CINNAMON CREAM LATTE</span>
                <span class="wm-row">CINNAMON CREAM LATTE</span>
                <span class="wm-row">CINNAMON CREAM LATTE</span>
                <span class="wm-row">CINNAMON CREAM LATTE</span>
            </h1>

            <div class="hero-body">
                <div class="hero-copy">
                    <p>Every cup at Azure is a small ceremony &mdash; sourced from the world's finest micro-farms, roasted in-house, and prepared by hands that genuinely care.</p>
                    <div class="hero-cta">
                        <a href="Coffee.aspx" class="btn btn-brown">Explore Menu</a>
                        <a href="#about" class="btn btn-glass">Our Story</a>
                    </div>
                </div>

                <div class="hero-image-wrap">
                    <img src="Images/LandingPage.png" alt="Cinnamon Cream Latte" />
                </div>

                <div class="hero-stats">
                    <div>
                        <span class="stat-num">12+</span>
                        <span class="stat-label">Origins</span>
                    </div>
                    <div>
                        <span class="stat-num">4.9&#9733;</span>
                        <span class="stat-label">Ratings</span>
                    </div>
                    <div>
                        <span class="stat-num">8YR</span>
                        <span class="stat-label">Crafting</span>
                    </div>
                </div>
            </div>
        </div>
    </section>


</asp:Content>
