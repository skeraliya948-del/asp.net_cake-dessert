<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SweetDelights.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- HERO SECTION -->
    <section class="relative overflow-hidden pt-10 pb-20 bg-gradient-to-b from-rose-50/70 via-white to-rose-50/40">
        <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
            <div class="grid lg:grid-cols-2 gap-12 items-center">
                
                <div class="space-y-6 text-center lg:text-left">
                    <div class="inline-flex items-center gap-2 px-3.5 py-1.5 rounded-full bg-rose-100 text-rose-800 text-xs font-semibold border border-rose-200 shadow-sm">
                        <span class="animate-pulse">🌸</span> Freshly Baked Artisanal Delights Daily
                    </div>
                    
                    <h1 class="font-serif-heading text-4xl sm:text-5xl lg:text-6xl font-bold text-slate-900 leading-tight">
                        Baked with <span class="bg-gradient-to-r from-rose-600 via-pink-600 to-amber-600 bg-clip-text text-transparent">Love</span>, Served with Pure Joy.
                    </h1>
                    
                    <p class="text-slate-600 text-base sm:text-lg max-w-xl mx-auto lg:mx-0">
                        Discover our signature Belgian chocolate truffle cakes, custom birthday creations, french macarons, and 100% eggless gourmet desserts delivered straight to your door.
                    </p>

                    <div class="flex flex-col sm:flex-row items-center justify-center lg:justify-start gap-4 pt-2">
                        <a href="Menu.aspx" class="w-full sm:w-auto bg-rose-600 hover:bg-rose-700 text-white font-bold px-8 py-3.5 rounded-full shadow-lg hover:shadow-rose-200 transition text-center flex items-center justify-center gap-2">
                            <i class="fa-solid fa-cake-candles"></i> Explore Full Menu
                        </a>
                        <a href="CustomCake.aspx" class="w-full sm:w-auto bg-amber-50 hover:bg-amber-100 text-amber-900 border border-amber-300 font-bold px-7 py-3.5 rounded-full shadow-sm hover:shadow transition text-center flex items-center justify-center gap-2">
                            <i class="fa-solid fa-wand-magic-sparkles text-amber-600"></i> Custom Cake Studio
                        </a>
                    </div>

                    <div class="pt-8 border-t border-rose-100 grid grid-cols-3 gap-4 text-center">
                        <div>
                            <span class="block text-2xl font-bold text-rose-700 font-serif-heading">100%</span>
                            <span class="text-xs text-slate-500 font-medium">Fresh & Eggless</span>
                        </div>
                        <div>
                            <span class="block text-2xl font-bold text-rose-700 font-serif-heading">4.9 ★</span>
                            <span class="text-xs text-slate-500 font-medium">Over 2,500+ Reviews</span>
                        </div>
                        <div>
                            <span class="block text-2xl font-bold text-rose-700 font-serif-heading">60 Min</span>
                            <span class="text-xs text-slate-500 font-medium">Express Home Delivery</span>
                        </div>
                    </div>

                </div>

                <div class="relative group">
                    <div class="absolute -inset-1.5 bg-gradient-to-r from-rose-400 to-amber-400 rounded-3xl blur-xl opacity-35 group-hover:opacity-50 transition duration-500"></div>
                    
                    <div class="relative rounded-3xl overflow-hidden shadow-2xl bg-white border border-rose-100 p-3">
                        <img src="https://images.unsplash.com/photo-1578985545062-69928b1d9587?auto=format&fit=crop&w=800&q=80" alt="Artisanal Celebration Cake" class="w-full h-[380px] sm:h-[460px] object-cover rounded-2xl">
                        
                        <div class="absolute bottom-6 left-6 right-6 sm:right-auto bg-white/95 backdrop-blur-md p-4 rounded-2xl shadow-xl border border-rose-100 flex items-center gap-3">
                            <div class="w-12 h-12 rounded-2xl bg-amber-100 flex items-center justify-center text-amber-600 text-xl font-bold flex-shrink-0">
                                👑
                            </div>
                            <div>
                                <h4 class="font-bold text-slate-800 text-sm">Royal Belgian Truffle Cake</h4>
                                <p class="text-xs text-slate-500">Rich Ganache & 24k Gold Flakes</p>
                                <span class="text-xs font-bold text-rose-600">₹650 / 0.5 kg</span>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </section>

    <!-- FEATURED CATEGORIES -->
    <section class="py-16 bg-white">
        <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
            <div class="text-center max-w-2xl mx-auto mb-12 space-y-3">
                <span class="text-xs font-bold uppercase tracking-widest text-rose-600 bg-rose-100 px-3 py-1 rounded-full">Explore Categories</span>
                <h2 class="font-serif-heading text-3xl sm:text-4xl font-bold text-slate-900">What Are You Craving Today?</h2>
            </div>

            <div class="grid sm:grid-cols-2 lg:grid-cols-4 gap-6">
                <a href="Menu.aspx" class="group bg-gradient-to-br from-rose-50 to-pink-50 p-6 rounded-3xl border border-rose-100/80 shadow-sm hover:shadow-xl transition text-center space-y-4">
                    <div class="w-16 h-16 rounded-2xl bg-rose-600 text-white flex items-center justify-center text-2xl mx-auto shadow-md group-hover:scale-110 transition">🎂</div>
                    <h3 class="font-serif-heading font-bold text-lg text-slate-900 group-hover:text-rose-600 transition">Celebration Cakes</h3>
                    <p class="text-slate-500 text-xs leading-relaxed">Birthday, anniversary & custom designer tiered cakes.</p>
                    <span class="inline-block text-xs font-bold text-rose-600 group-hover:underline">Explore Cakes &rarr;</span>
                </a>

                <a href="Menu.aspx" class="group bg-gradient-to-br from-amber-50 to-orange-50 p-6 rounded-3xl border border-amber-100/80 shadow-sm hover:shadow-xl transition text-center space-y-4">
                    <div class="w-16 h-16 rounded-2xl bg-amber-500 text-white flex items-center justify-center text-2xl mx-auto shadow-md group-hover:scale-110 transition">🍰</div>
                    <h3 class="font-serif-heading font-bold text-lg text-slate-900 group-hover:text-amber-600 transition">Gourmet Pastries</h3>
                    <p class="text-slate-500 text-xs leading-relaxed">Single slices, cheesecakes & jar desserts for quick cravings.</p>
                    <span class="inline-block text-xs font-bold text-amber-600 group-hover:underline">Explore Slices &rarr;</span>
                </a>

                <a href="Menu.aspx" class="group bg-gradient-to-br from-pink-50 to-purple-50 p-6 rounded-3xl border border-pink-100/80 shadow-sm hover:shadow-xl transition text-center space-y-4">
                    <div class="w-16 h-16 rounded-2xl bg-pink-500 text-white flex items-center justify-center text-2xl mx-auto shadow-md group-hover:scale-110 transition">🧁</div>
                    <h3 class="font-serif-heading font-bold text-lg text-slate-900 group-hover:text-pink-600 transition">French Macarons</h3>
                    <p class="text-slate-500 text-xs leading-relaxed">Authentic french macaron gift boxes & dessert cups.</p>
                    <span class="inline-block text-xs font-bold text-pink-600 group-hover:underline">Explore Macarons &rarr;</span>
                </a>

                <a href="CustomCake.aspx" class="group bg-gradient-to-br from-purple-50 to-rose-50 p-6 rounded-3xl border border-purple-100/80 shadow-sm hover:shadow-xl transition text-center space-y-4">
                    <div class="w-16 h-16 rounded-2xl bg-purple-600 text-white flex items-center justify-center text-2xl mx-auto shadow-md group-hover:scale-110 transition">✨</div>
                    <h3 class="font-serif-heading font-bold text-lg text-slate-900 group-hover:text-purple-600 transition">Custom Cake Studio</h3>
                    <p class="text-slate-500 text-xs leading-relaxed">Design your own cake with instant size & pricing calculator.</p>
                    <span class="inline-block text-xs font-bold text-purple-600 group-hover:underline">Build Custom Cake &rarr;</span>
                </a>
            </div>
        </div>
    </section>
</asp:Content>
