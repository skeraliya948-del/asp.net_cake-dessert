<%@ Page Language="C#" AutoEventWireup="true" ResponseEncoding="UTF-8" ContentType="text/html; charset=utf-8" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>About Us – Sweet Layers</title>
    <link rel="stylesheet" href="Styles/main.css" />
</head>
<body>

    <!-- ===== NAVBAR ===== -->
    <nav class="navbar">
        <div class="brand">&#127874; Sweet<span>Layers</span></div>
        <ul>
            <li><a href="Default.aspx">Home</a></li>
            <li><a href="CakeMenu.aspx">Cake Menu</a></li>
            <li><a href="About.aspx" class="active">About</a></li>
            <li><a href="Registration.aspx">Register</a></li>
            <li><a href="ContactUs.aspx">Contact Us</a></li>
        </ul>
    </nav>

    <!-- ===== ABOUT HEADER ===== -->
    <div class="about-header">
        <h1>&#127874; About Sweet Layers</h1>
        <p>Our story, our passion, and the people who make the magic happen.</p>
    </div>

    <!-- ===== OUR STORY ===== -->
    <section class="about-story-section">
        <div class="about-story-wrap">
            <div class="about-story-img">
                <img src="https://images.unsplash.com/photo-1486427944299-d1955d23e34d?w=600&h=420&fit=crop"
                     alt="Baking in kitchen"
                     onerror="this.src='https://via.placeholder.com/600x420/7b3f00/fff?text=Our+Story'" />
            </div>
            <div class="about-story-text">
                <h2>Our Story</h2>
                <p>Sweet Layers was born in 2015 from a simple kitchen and a big dream. What started as a home-baking hobby quickly grew into one of the most loved cake shops, thanks to our commitment to quality, creativity, and great taste.</p>
                <p>Every cake we bake is made from scratch using the finest ingredients — no shortcuts, no preservatives. We believe that a cake is not just dessert; it's a memory in the making.</p>
                <p>Today we serve thousands of happy customers every year, delivering joy to birthdays, weddings, anniversaries, and everyday celebrations.</p>
            </div>
        </div>
    </section>

    <!-- ===== STATS ===== -->
    <section class="about-stats-section">
        <div class="about-stat">
            <div class="stat-number">10+</div>
            <div class="stat-label">Years of Baking</div>
        </div>
        <div class="about-stat">
            <div class="stat-number">50K+</div>
            <div class="stat-label">Happy Customers</div>
        </div>
        <div class="about-stat">
            <div class="stat-number">100+</div>
            <div class="stat-label">Cake Varieties</div>
        </div>
        <div class="about-stat">
            <div class="stat-number">15+</div>
            <div class="stat-label">Cities Served</div>
        </div>
    </section>

    <!-- ===== OUR VALUES ===== -->
    <section class="about-values-section">
        <h2 class="section-title">What We Stand For</h2>
        <div class="about-values-grid">
            <div class="about-value-card">
                <div class="av-icon">&#129361;</div>
                <h4>Freshness First</h4>
                <p>Every cake is baked fresh the same day — never stored, never frozen.</p>
            </div>
            <div class="about-value-card">
                <div class="av-icon">&#127775;</div>
                <h4>Premium Quality</h4>
                <p>We source only the best ingredients — Belgian chocolate, fresh dairy, real fruit.</p>
            </div>
            <div class="about-value-card">
                <div class="av-icon">&#127912;</div>
                <h4>Artisan Craftsmanship</h4>
                <p>Every design is hand-crafted with love and artistry by our expert bakers.</p>
            </div>
            <div class="about-value-card">
                <div class="av-icon">&#128149;</div>
                <h4>Made with Love</h4>
                <p>We put heart into every cake because we know it's part of your special moment.</p>
            </div>
        </div>
    </section>

    <!-- ===== MEET THE TEAM ===== -->
    <section class="about-team-section">
        <h2 class="section-title">Meet Our Bakers</h2>
        <div class="team-grid">

            <div class="team-card">
                <img src="https://images.unsplash.com/photo-1607631568010-a87245c0daf4?w=300&h=300&fit=crop"
                     alt="Priya Sharma"
                     onerror="this.src='https://via.placeholder.com/300x300/bf6b1a/fff?text=Priya'" />
                <h4>Priya Sharma</h4>
                <span class="team-role">Head Baker &amp; Founder</span>
                <p>15 years of baking experience. Trained in Paris and loves chocolate above all else.</p>
            </div>

            <div class="team-card">
                <img src="https://images.unsplash.com/photo-1542314831-068cd1dbfeeb?w=300&h=300&fit=crop"
                     alt="Rahul Mehta"
                     onerror="this.src='https://via.placeholder.com/300x300/7b3f00/fff?text=Rahul'" />
                <h4>Rahul Mehta</h4>
                <span class="team-role">Pastry Chef</span>
                <p>Specialises in custom wedding cakes and fondant art with over 10 years experience.</p>
            </div>

            <div class="team-card">
                <img src="https://images.unsplash.com/photo-1551836022-4c4c79ecde51?w=300&h=300&fit=crop"
                     alt="Sneha Patel"
                     onerror="this.src='https://via.placeholder.com/300x300/e8a44a/fff?text=Sneha'" />
                <h4>Sneha Patel</h4>
                <span class="team-role">Cake Decorator</span>
                <p>Her floral and watercolour cake designs have earned thousands of fans on social media.</p>
            </div>

        </div>
    </section>

    <!-- ===== FOOTER ===== -->
    <footer class="footer">
        <p>&copy; 2024 Sweet Layers Cake Shop. Made with <span>&#10084;</span> for cake lovers.</p>
    </footer>

</body>
</html>
