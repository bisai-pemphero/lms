<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>NASI Holdings - Management Systems</title>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css">
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700&display=swap" rel="stylesheet">
    <style>
        :root {
            --primary: #2563eb;
            --primary-dark: #1d4ed8;
            --secondary: #64748b;
            --dark: #1e293b;
            --light: #f8fafc;
            --accent-blue: #3b82f6;
            --accent-green: #10b981;
            --accent-orange: #f97316;
            --card-shadow: 0 10px 25px -5px rgba(0, 0, 0, 0.1);
            --transition: all 0.3s ease;
        }
        
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
            font-family: 'Inter', -apple-system, BlinkMacSystemFont, sans-serif;
        }
        
        body {
            background-color: #f1f5f9;
            color: var(--dark);
            line-height: 1.6;
            min-height: 100vh;
            display: flex;
            flex-direction: column;
        }
        
        .container {
            max-width: 1200px;
            margin: 0 auto;
            padding: 20px;
        }
        
        /* Header Styles */
        header {
            background: linear-gradient(120deg, var(--dark) 0%, #0f172a 100%);
            color: white;
            padding: 1.2rem 0;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
            position: sticky;
            top: 0;
            z-index: 100;
        }
        
        .header-content {
            display: flex;
            justify-content: space-between;
            align-items: center;
        }
        
        .logo {
            display: flex;
            align-items: center;
            gap: 15px;
        }
        
        .logo img {
            height: 45px;
        }
        
        .logo-text {
            font-size: 1.6rem;
            font-weight: 700;
            letter-spacing: -0.5px;
        }
        
        .logo-text span {
            color: var(--accent-blue);
            font-weight: 800;
        }
        
        nav ul {
            display: flex;
            list-style: none;
            gap: 28px;
        }
        
        nav a {
            color: #e2e8f0;
            text-decoration: none;
            font-weight: 500;
            transition: var(--transition);
            font-size: 0.95rem;
            position: relative;
            padding: 0.5rem 0;
        }
        
        nav a:after {
            content: '';
            position: absolute;
            width: 0;
            height: 2px;
            bottom: 0;
            left: 0;
            background-color: var(--accent-blue);
            transition: var(--transition);
        }
        
        nav a:hover {
            color: white;
        }
        
        nav a:hover:after {
            width: 100%;
        }
        
        /* Hero Section */
        .hero {
            text-align: center;
            padding: 5rem 0 3rem;
            position: relative;
            overflow: hidden;
        }
        
        .hero:before {
            content: '';
            position: absolute;
            top: -100px;
            right: -100px;
            width: 300px;
            height: 300px;
            border-radius: 50%;
            background: linear-gradient(45deg, rgba(59, 130, 246, 0.1) 0%, rgba(37, 99, 235, 0.05) 100%);
            z-index: -1;
        }
        
        .hero:after {
            content: '';
            position: absolute;
            bottom: -50px;
            left: -50px;
            width: 200px;
            height: 200px;
            border-radius: 50%;
            background: linear-gradient(45deg, rgba(16, 185, 129, 0.1) 0%, rgba(37, 99, 235, 0.05) 100%);
            z-index: -1;
        }
        
        .hero h1 {
            font-size: 2.8rem;
            margin-bottom: 1rem;
            color: var(--dark);
            font-weight: 800;
            letter-spacing: -0.5px;
        }
        
        .hero p {
            font-size: 1.1rem;
            max-width: 700px;
            margin: 0 auto 3rem;
            color: var(--secondary);
            font-weight: 400;
        }
        
        /* Cards Section */
        .cards {
            display: flex;
            justify-content: center;
            gap: 30px;
            flex-wrap: wrap;
            margin: 2rem 0 4rem;
        }
        
        .card {
            background: white;
            border-radius: 16px;
            overflow: hidden;
            width: 380px;
            box-shadow: var(--card-shadow);
            transition: var(--transition);
            border: 1px solid rgba(226, 232, 240, 0.8);
        }
        
        .card:hover {
            transform: translateY(-8px);
            box-shadow: 0 20px 40px -10px rgba(0, 0, 0, 0.15);
        }
        
        .card-header {
            padding: 2rem;
            text-align: center;
            color: white;
            position: relative;
        }
        
        .card-header i {
            font-size: 2.5rem;
            margin-bottom: 1rem;
            opacity: 0.9;
        }
        
        .card-header h2 {
            font-size: 1.6rem;
            margin-bottom: 0.5rem;
            font-weight: 700;
        }
        
        .card-header p {
            opacity: 0.9;
            font-weight: 300;
        }
        
        .nasi .card-header {
            background: linear-gradient(120deg, var(--primary) 0%, var(--primary-dark) 100%);
        }
        
        .innobuild .card-header {
            background: linear-gradient(120deg, var(--accent-green) 0%, #0d9665 100%);
        }
        
        .innobuild-old .card-header {
            background: linear-gradient(120deg, var(--accent-orange) 0%, #d1580c 100%);
        }
        
        .card-body {
            padding: 2rem;
        }
        
        .card-body ul {
            list-style: none;
            margin-bottom: 2rem;
        }
        
        .card-body li {
            padding: 0.8rem 0;
            border-bottom: 1px solid #f1f5f9;
            display: flex;
            align-items: center;
            font-size: 0.95rem;
        }
        
        .card-body li:last-child {
            border-bottom: none;
        }
        
        .card-body li i {
            color: var(--primary);
            margin-right: 12px;
            font-size: 1.1rem;
        }
        
        .btn {
            display: block;
            text-align: center;
            padding: 1rem;
            border-radius: 8px;
            color: white;
            text-decoration: none;
            font-weight: 600;
            transition: var(--transition);
            font-size: 1rem;
        }
        
        .btn i {
            margin-left: 8px;
            font-size: 0.9rem;
        }
        
        .btn-nasi {
            background: var(--primary);
        }
        
        .btn-nasi:hover {
            background: var(--primary-dark);
        }
        
        .btn-innobuild {
            background: var(--accent-green);
        }
        
        .btn-innobuild:hover {
            background: #0d9665;
        }
        
        .btn-innobuild-old {
            background: var(--accent-orange);
        }
        
        .btn-innobuild-old:hover {
            background: #d1580c;
        }
        
        /* Features Section */
        .features {
            background: white;
            border-radius: 16px;
            padding: 3.5rem 2.5rem;
            box-shadow: var(--card-shadow);
            margin: 3rem 0;
            border: 1px solid rgba(226, 232, 240, 0.8);
        }
        
        .features h2 {
            text-align: center;
            margin-bottom: 3rem;
            color: var(--dark);
            font-size: 2.2rem;
            font-weight: 700;
        }
        
        .feature-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
            gap: 2.5rem;
        }
        
        .feature {
            display: flex;
            gap: 1.5rem;
        }
        
        .feature-icon {
            background: var(--primary);
            color: white;
            width: 60px;
            height: 60px;
            border-radius: 12px;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 1.5rem;
            flex-shrink: 0;
        }
        
        .feature-content h3 {
            margin-bottom: 0.8rem;
            color: var(--dark);
            font-weight: 600;
        }
        
        .feature-content p {
            color: var(--secondary);
            font-size: 0.95rem;
            line-height: 1.7;
        }
        
        /* Footer */
        footer {
            background: var(--dark);
            color: white;
            padding: 3.5rem 0 1.5rem;
            margin-top: auto;
        }
        
        .footer-content {
            display: flex;
            justify-content: space-between;
            flex-wrap: wrap;
            gap: 2.5rem;
        }
        
        .footer-section {
            flex: 1;
            min-width: 250px;
        }
        
        .footer-section h3 {
            margin-bottom: 1.5rem;
            font-size: 1.3rem;
            font-weight: 600;
        }
        
        .footer-section p, .footer-section li {
            margin-bottom: 0.8rem;
            color: #cbd5e1;
            font-size: 0.9rem;
        }
        
        .footer-section ul {
            list-style: none;
        }
        
        .footer-section a {
            color: var(--accent-blue);
            text-decoration: none;
            transition: var(--transition);
        }
        
        .footer-section a:hover {
            color: white;
            text-decoration: underline;
        }
        
        .copyright {
            text-align: center;
            padding-top: 2.5rem;
            margin-top: 2.5rem;
            border-top: 1px solid #334155;
            color: #94a3b8;
            font-size: 0.85rem;
        }
        
        /* Responsive Design */
        @media (max-width: 768px) {
            .header-content {
                flex-direction: column;
                gap: 1.2rem;
            }
            
            nav ul {
                gap: 1.2rem;
                flex-wrap: wrap;
                justify-content: center;
            }
            
            .hero h1 {
                font-size: 2.2rem;
            }
            
            .hero p {
                font-size: 1rem;
                padding: 0 1rem;
            }
            
            .card {
                width: 100%;
                max-width: 400px;
            }
            
            .feature {
                flex-direction: column;
                text-align: center;
            }
            
            .feature-icon {
                align-self: center;
            }
        }
        
        /* Animation Classes */
        .fade-in {
            opacity: 0;
            transform: translateY(20px);
            transition: opacity 0.6s ease, transform 0.6s ease;
        }
        
        .fade-in.appear {
            opacity: 1;
            transform: translateY(0);
        }
    </style>
</head>
<body>
   

    <div class="container">
        <section class="hero">
            <h1 class="fade-in">Nasi Holdings Management Information Systems</h1>
           
            <div class="cards">
                <div class="card nasi fade-in">
                   
                    <div class="card-body">
                       Nasi Enterprise
                        <br />
                        <a href="http://102.218.160.99/nasi/UserLogin.aspx" class="btn btn-nasi">Access System <i class="fas fa-arrow-right"></i></a>
                    </div>
                </div>
                
                <div class="card innobuild fade-in">
                   
                    <div class="card-body">
                       Innobuild MIS <br />
                        <a href="http://102.218.160.99/lms/UserLogin.aspx" class="btn btn-innobuild">Access System <i class="fas fa-arrow-right"></i></a>
                    </div>
                </div>
                
                <div class="card innobuild-old fade-in">
                   
                    <div class="card-body">
                       Innobuild MIS (Old)
                        <a href="http://102.218.160.99/Innobuild/Login.aspx" class="btn btn-innobuild-old">Access Legacy System <i class="fas fa-arrow-right"></i></a>
                    </div>
                </div>
            </div>
        </section>

       
    </div>

    
    <script>
        // Scroll animation for elements
        document.addEventListener('DOMContentLoaded', function () {
            const fadeElements = document.querySelectorAll('.fade-in');

            const appearOptions = {
                threshold: 0.15,
                rootMargin: "0px 0px -100px 0px"
            };

            const appearOnScroll = new IntersectionObserver(function (entries, appearOnScroll) {
                entries.forEach(entry => {
                    if (!entry.isIntersecting) {
                        return;
                    } else {
                        entry.target.classList.add('appear');
                        appearOnScroll.unobserve(entry.target);
                    }
                });
            }, appearOptions);

            fadeElements.forEach(element => {
                appearOnScroll.observe(element);
            });

            // Add slight delay to hero elements for staged animation
            const heroElements = document.querySelectorAll('.hero .fade-in');
            heroElements.forEach((el, index) => {
                el.style.transitionDelay = `${index * 0.2}s`;
            });
        });
    </script>
</body>
</html>