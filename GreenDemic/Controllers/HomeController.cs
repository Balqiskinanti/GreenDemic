using Greendemic.DAL;
using Greendemic.Models;
using GreenDemic.DAL;
using GreenDemic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using MimeKit;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GreenDemic.Controllers
{
    public class HomeController : Controller
    {
        // DAL context
        private AccountDAL accountContext = new AccountDAL();
        private PersonDAL personContext = new PersonDAL();
        private ItemDAL itemContext = new ItemDAL();
        private ShoppingBagItemDAL shoppingBagItemContext = new ShoppingBagItemDAL();
        private ShoppingBagDAL shoppingBagContext = new ShoppingBagDAL();

        //Logging
        private readonly ILogger _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Index
        public IActionResult Index()
        {
            List<Account> topAccs = accountContext.GetTop3Users();
            ViewData["FirstName"] = topAccs[0].AccName;
            ViewData["FirstData"] = topAccs[0].Health + " Health | " + topAccs[0].QuizPoints + " Points";
            ViewData["Badge1"] = GetPointsBadge(topAccs[0].QuizPoints);
            ViewData["Streak1"] = IsHealthStreakComplete(topAccs[0].Health);

            ViewData["SecondName"] = topAccs[1].AccName;
            ViewData["SecondData"] = topAccs[1].Health + " Health | " + topAccs[1].QuizPoints + " Points";
            ViewData["Badge2"] = GetPointsBadge(topAccs[1].QuizPoints);
            ViewData["Streak2"] = IsHealthStreakComplete(topAccs[1].Health);

            ViewData["ThirdName"] = topAccs[2].AccName;
            ViewData["ThirdData"] = topAccs[2].Health + " Health | " + topAccs[2].QuizPoints + " Points";
            ViewData["Badge3"] = GetPointsBadge(topAccs[2].QuizPoints);
            ViewData["Streak3"] = IsHealthStreakComplete(topAccs[2].Health);

            return View();
        }

        // Get badges by user's points
        private int GetPointsBadge(int userPts)
        {
            int points;
            if (userPts > 500)
            {
                points = 1;
                if (userPts > 1000)
                {
                    points = 2;
                    if (userPts > 1500)
                    {
                        points = 3;
                    }
                }
            }
            else
            {
                points = 0;
            }

            return points;
        }

        // Check if streak is complete i.e. 12/12 
        private int IsHealthStreakComplete(int userHealth)
        {
            int streak = 0;
            if(userHealth == 12)
            {
                streak = 1;
            }
            return streak;
        }

        private int IsTop3(Account user)
        {
            int istop3 = 0;
            List<Account> topAccs = accountContext.GetTop3Users();
            if(user.AccID == topAccs[0].AccID || user.AccID == topAccs[1].AccID || user.AccID == topAccs[2].AccID)
            {
                istop3 = 1;
            }
            return istop3;

        }

        // Privacy
        public IActionResult Privacy()
        {
            return View();
        }

        //About Us
        public IActionResult AboutUs()
        {
            return View();
        }

        //Settings
        public ActionResult Settings()
        {
            // Authenticate user
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "User"))
            {
                return RedirectToAction("Index", "Home");
            }

            //Account users' info
            int accID = (int)HttpContext.Session.GetInt32("AccID");
            Account user = accountContext.GetDetails(accID);
            ViewData["ID"] = user.AccID;
            ViewData["Name"] = user.AccName;
            ViewData["Password"] = user.Pass_word;
            ViewData["Email"] = user.UserName;
            ViewData["Health"] = user.Health;
            ViewData["Quiz"] = user.QuizPoints;
            ViewData["Mute"] = user.IsMuted;
            ViewData["Bio"] = user.Bio;
            ViewData["IsTop3"] = IsTop3(user) ;
            ViewData["Badge"] = GetPointsBadge(user.QuizPoints);
            ViewData["Streak"] = IsHealthStreakComplete(user.Health);
            if(user.IsMuted)
            {
                ViewData["Mute"] = true;
            }
            if (!user.IsMuted)
            {
                ViewData["Mute"] = false;
            }

            if (user.Bio is null)
            {
                ViewData["Bio"] = "-";
            }

            UpdateHealth();

            return View();
        }

        [HttpPost]
        public ActionResult Settings(IFormCollection formData, string IsBioChanged)
        {
            // Authenticate user
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "User"))
            {
                return RedirectToAction("Index", "Home");
            }

            //Account users' info
            int accID = (int)HttpContext.Session.GetInt32("AccID");
            Account user = accountContext.GetDetails(accID);

            string name = formData["name"].ToString();
            string email = formData["email"].ToString();
            string password = formData["password"].ToString();
            string bio = formData["bio"].ToString();
            string mute = formData["IsMuted"].ToString().ToLower();
            string prevMute = accountContext.GetDetails(accID).IsMuted.ToString().ToLower();

            if (name != "")
            {
                user.AccName = name;
            }
            if (email != "")
            {
                user.UserName = email;
            }
            if (password != "")
            {
                user.Pass_word = password;
            }
            if (bio != "")
            {
                user.Bio = bio;
            }
            if (bio == "" && IsBioChanged == "yes")
            {
                user.Bio = null;
            }
            if (IsBioChanged == null && mute != prevMute)
            {
                user.IsMuted = !user.IsMuted;
            }

            HttpContext.Session.SetString("AccName", user.AccName);
            accountContext.Update(user);
            ViewData["ID"] = user.AccID;
            ViewData["Name"] = user.AccName;
            ViewData["Password"] = user.Pass_word;
            ViewData["Email"] = user.UserName;
            ViewData["Health"] = user.Health;
            ViewData["Quiz"] = user.QuizPoints;
            ViewData["Mute"] = user.IsMuted;
            ViewData["Bio"] = user.Bio;
            ViewData["IsTop3"] = IsTop3(user);
            ViewData["Badge"] = GetPointsBadge(user.QuizPoints);
            ViewData["Streak"] = IsHealthStreakComplete(user.Health);

            if (user.Bio is null)
            {
                ViewData["Bio"] = "-";
            }
            if (user.IsMuted)
            {
                ViewData["Mute"] = true;
            }
            if (!user.IsMuted)
            {
                ViewData["Mute"] = false;
            }
            return View();
        }

        // Quiz for users
        public ActionResult Quiz()
        {
            // Authenticate user
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "User"))
            {
                return RedirectToAction("Index", "Home");
            }

            int accID = (int)HttpContext.Session.GetInt32("AccID");
            Account user = accountContext.GetDetails(accID);
            int points = user.QuizPoints;
            ViewData["Points"] = points;
            return View();
        }

        // Update quiz points
        [HttpPost]
        public ActionResult Quiz(int? points)
        {
            // Authenticate user
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "User"))
            {
                return RedirectToAction("Index", "Home");
            }

            int accID = (int)HttpContext.Session.GetInt32("AccID");
            Account user = accountContext.GetDetails(accID);
            user.QuizPoints += points.Value;
            accountContext.Update(user);
            return RedirectToAction("Main");
        }

        // Return Type of Email for dropdown list
        private List<SelectListItem> GetTypeEmail()
        {
            List<SelectListItem> categoryList = new List<SelectListItem>();
            List<String> myCategoryList = new List<string>() { "Top 3", "Bottom 3", "All" };

            foreach (String cat in myCategoryList)
            {
                categoryList.Add(
                    new SelectListItem
                    {
                        Value = cat,
                        Text = cat
                    });
            }
            return categoryList;
        }

        // Send Email
        public ActionResult SendMail()
        {
            // Authenticate user
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["GetType"] = GetTypeEmail();
            return View();
        }

        // Send Email based on type : top 3, bottom 3, all
        [HttpPost]
        public ActionResult SendMail(Message m)
        {
            try
            {
                MimeMessage message = new MimeMessage();

                // From
                MailboxAddress from = new MailboxAddress("GreenDemic",
                "contact.greendemic@gmail.com");
                message.From.Add(from);

                // To
                List<String> bottom3Emails = accountContext.GetBottom3Emails();
                List<String> top3Emails = accountContext.GetTop3Emails();
                List<String> allEmails = accountContext.GetAllEmails();

                if (m.Type == "Bottom 3")
                {
                    int cnt = 0;
                    foreach (String email in bottom3Emails)
                    {
                        cnt++;
                        if (cnt > 3)
                        {
                            break;
                        }
                        MailboxAddress to = new MailboxAddress("", email);
                        message.To.Add(to);
                    }
                }
                else if (m.Type == "Top 3")
                {
                    int cnt = 0;
                    foreach (String email in top3Emails)
                    {
                        cnt++;
                        if (cnt > 3)
                        {
                            break;
                        }
                        MailboxAddress to = new MailboxAddress("", email);
                        message.To.Add(to);
                    }
                }
                else
                {
                    foreach (String email in allEmails)
                    {
                        MailboxAddress to = new MailboxAddress("", email);
                        message.To.Add(to);
                    }
                }

                // Subject
                message.Subject = m.Subject;

                // Body
                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = "<p style='font-size:20px;'><b>Hello! GreenDemic Team Hopes You Are Well!</b><br/> " + m.Content + " </p><br/>" +
                    "<img src='https://pbs.twimg.com/media/D9_tuAfWwAEKJKJ.png'/><br/>" +
                    "<p>Image from: https://pbs.twimg.com/media/D9_tuAfWwAEKJKJ.png </p>";
                message.Body = bodyBuilder.ToMessageBody();

                // SMTP Client
                MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient();
                client.Connect("smtp.gmail.com", 465, true);
                client.Authenticate("contact.greendemic@gmail.com", "GreenDemic_2021");
                client.Send(message);
                client.Disconnect(true);
                client.Dispose();

                // Dropdown
                ViewData["GetType"] = GetTypeEmail();
                return RedirectToAction("AdminMain");
            }
            catch
            {
                RedirectToAction("AdminMain");
            }
            return View();
        } 

        // Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // GET: HomeController/Login
        public IActionResult Login()
        {
            ViewData["IsFailed"] = false;
            return View();
        }

        // POST: HomeController/Login
        [HttpPost]
        public ActionResult Login(IFormCollection formData)
        {
            // Read inputs from textboxes
            string username = formData["txtEmail"].ToString().ToLower();
            string password = formData["txtPassword"].ToString();

            ViewData["IsFailed"] = false;
            bool isUser = false;
            bool isAdmin = false;
            try
            {
                foreach (Account a in accountContext.GetAllAccounts())
                {
                    // General user
                    if (username == a.UserName.ToLower() && password == a.Pass_word)
                    {
                        HttpContext.Session.SetString("AccName", a.AccName);
                        HttpContext.Session.SetInt32("AccID", a.AccID);
                        HttpContext.Session.SetString("Role", "User");
                        isUser = true;
                        break;
                    }
                    // Admin
                    else if (username == "contact.greendemic@gmail.com" && password == "1234")
                    {
                        HttpContext.Session.SetString("AccName", "Admin");
                        HttpContext.Session.SetString("Role", "Admin");

                        isAdmin = true;
                        _logger.LogInformation("Admin");
                        break;
                    }
                }
                // if user
                if (isUser == true)
                {
                    return RedirectToAction("LandingMain");
                }
                if (isAdmin == true)
                {
                    return RedirectToAction("AdminMain");
                }
                // if not a user (wrong credentials)
                else
                {
                    TempData["Message"] = "Invalid Login Credentials!";
                    return RedirectToAction("Login");
                }
            }
            catch
            {
                ViewData["IsFailed"] = true;
                return RedirectToAction("Login");
            }
        }

        //GET: HomeController/SignUp
        public IActionResult SignUp()
        {
            ViewData["IsFailed"] = false;
            return View();
        }

        // POST: HomeController/SignUp
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignUp(Account account)
        {
            ViewData["IsFailed"] = false;
            try
            {
                account.Bio = null;
                account.Health = 12;
                account.IsMuted = false;
                account.QuizPoints = 0;
                if (ModelState.IsValid)
                {
                    account.AccID = accountContext.Add(account);
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(account);
                }
            }
            catch
            {
                ViewData["IsFailed"] = true;
                return View(account);
            }
        }

        // Admin Main page
        public ActionResult AdminMain()
        {
            // Authenticate user
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            
            // Get top accounts
            List<Account> topAccs = accountContext.GetTop3Users();
            ViewData["FirstName"] = topAccs[0].AccName;
            ViewData["FirstData"] = topAccs[0].Health + " Health | " + topAccs[0].QuizPoints + " Points";

            ViewData["SecondName"] = topAccs[1].AccName;
            ViewData["SecondData"] = topAccs[1].Health + " Health | " + topAccs[1].QuizPoints + " Points";

            ViewData["ThirdName"] = topAccs[2].AccName;
            ViewData["ThirdData"] = topAccs[2].Health + " Health | " + topAccs[2].QuizPoints + " Points";

            // Get bottom accounts
            List<Account> bottomAccs = accountContext.GetBottom3Users();
            ViewData["BottomFirstName"] = bottomAccs[0].AccName;
            ViewData["BottomFirstData"] = bottomAccs[0].Health + " Health | " + bottomAccs[0].QuizPoints + " Points";

            ViewData["BottomSecondName"] = bottomAccs[1].AccName;
            ViewData["BottomSecondData"] = bottomAccs[1].Health + " Health | " + bottomAccs[1].QuizPoints + " Points";

            ViewData["BottomThirdName"] = bottomAccs[2].AccName;
            ViewData["BottomThirdData"] = bottomAccs[2].Health + " Health | " + bottomAccs[2].QuizPoints + " Points";

            // Get summary of accounts' calories per month
            List<Account> accList = accountContext.GetAllAccounts();
            List<int> thisYearCalList = new List<int>() { 0,0,0,0,0,0,0,0,0,0,0,0};
            foreach(Account a in accList)
            {
                List<ShoppingBag> sbList = shoppingBagContext.GetAllShoppingBags(a.AccID);
                foreach(ShoppingBag s in sbList)
                {
                    ShoppingBagViewModel sbvm = MapToShoppingBagVM(s);
                    if(sbvm.CreatedAt.Month == 1)
                    {
                        int totalCal = sbvm.totalCals;
                        thisYearCalList[0] += totalCal;
                    }
                    if (sbvm.CreatedAt.Month == 2)
                    {
                        int totalCal = sbvm.totalCals;
                        thisYearCalList[1] += totalCal;
                    }
                    if (sbvm.CreatedAt.Month == 3)
                    {
                        int totalCal = sbvm.totalCals;
                        thisYearCalList[2] += totalCal;
                    }
                    if (sbvm.CreatedAt.Month == 4)
                    {
                        int totalCal = sbvm.totalCals;
                        thisYearCalList[3] += totalCal;
                    }
                    if (sbvm.CreatedAt.Month == 5)
                    {
                        int totalCal = sbvm.totalCals;
                        thisYearCalList[4] += totalCal;
                    }
                    if (sbvm.CreatedAt.Month == 6)
                    {
                        int totalCal = sbvm.totalCals;
                        thisYearCalList[5] += totalCal;
                    }
                    if (sbvm.CreatedAt.Month == 7)
                    {
                        int totalCal = sbvm.totalCals;
                        thisYearCalList[6] += totalCal;
                    }
                    if (sbvm.CreatedAt.Month == 8)
                    {
                        int totalCal = sbvm.totalCals;
                        thisYearCalList[7] += totalCal;
                    }
                    if (sbvm.CreatedAt.Month == 9)
                    {
                        int totalCal = sbvm.totalCals;
                        thisYearCalList[8] += totalCal;
                    }
                    if (sbvm.CreatedAt.Month == 10)
                    {
                        int totalCal = sbvm.totalCals;
                        thisYearCalList[9] += totalCal;
                    }
                    if (sbvm.CreatedAt.Month == 11)
                    {
                        int totalCal = sbvm.totalCals;
                        thisYearCalList[10] += totalCal;
                    }
                    if (sbvm.CreatedAt.Month == 12)
                    {
                        int totalCal = sbvm.totalCals;
                        thisYearCalList[11] += totalCal;
                    }
                }
            }
            ViewData["ThisYearCals"] = thisYearCalList;
            return View();
        }

        // GET: HomeController/Main
        public IActionResult Main()
        {
            // Authenticate user
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "User"))
            {
                return RedirectToAction("Index", "Home");
            }

            //Account users' total calories
            int accID = (int)HttpContext.Session.GetInt32("AccID");
            List<Person> personList = personContext.GetAllPerson(accID);
            int totalCal = CalculateTotalCalories(personList);
            HttpContext.Session.SetInt32("TotalCal", totalCal);

            //This month's total calories
            List<ShoppingBag> bagList = shoppingBagContext.GetThisMonthBags(accID);
            int thisMonthCal = CalculateMonthCalories(bagList);
            ViewData["ThisMonthCals"] = thisMonthCal;

            //Get total calories of each categories this month
            List<ShoppingBagItem> sbItemList_indiv = new List<ShoppingBagItem>();
            List<ShoppingBagItem> sbItemList = new List<ShoppingBagItem>();
            List<int> sbIDList = new List<int>();
            List<int> itemIDList = new List<int>();
            int snackCals = 0;
            int meatCals = 0;
            int seafoodCals = 0;
            int dairyCals = 0;
            int grainsCals = 0;
            int fruitCals = 0;
            int vegeCals = 0;
            int othersCals = 0;

            foreach (ShoppingBag s in bagList)
            {
                sbIDList.Add(s.ShoppingBagID); // get shopping bag ID list
            }

            foreach (int sbID in sbIDList)
            {
                sbItemList_indiv = shoppingBagItemContext.GetAllShoppingBagItem(sbID); // get list of shopping bag item
                foreach(ShoppingBagItem sbi in sbItemList_indiv)
                {
                    sbItemList.Add(sbi);
                }
            }
            
            foreach (ShoppingBagItem sbItem in sbItemList)
            {
                itemIDList.Add(sbItem.ItemID);
            }

            foreach (ShoppingBagItem s in sbItemList)
            {
                Item item = itemContext.GetDetails(s.ItemID);
                if (item.Category == "Snack")
                {
                    snackCals += (item.Cal * s.Qty);
                }
                else if (item.Category == "Meat")
                {
                    meatCals += (item.Cal * s.Qty);
                }
                else if (item.Category == "Seafood")
                {
                    seafoodCals += (item.Cal * s.Qty);
                }
                else if (item.Category == "Dairy")
                {
                    dairyCals += (item.Cal * s.Qty);
                }
                else if (item.Category == "Grains")
                {
                    grainsCals += (item.Cal * s.Qty);
                }
                else if (item.Category == "Fruit")
                {
                    fruitCals += (item.Cal * s.Qty);
                }
                else if (item.Category == "Vegetable")
                {
                    vegeCals += (item.Cal * s.Qty);
                }
                else if (item.Category == "Others")
                {
                    othersCals += (item.Cal * s.Qty);
                }
            }
            ViewData["Snack"] = snackCals;
            ViewData["Meat"] = meatCals;
            ViewData["Seafood"] = seafoodCals;
            ViewData["Dairy"] = dairyCals;
            ViewData["Grains"] = grainsCals;
            ViewData["Fruit"] = fruitCals;
            ViewData["Vegetable"] = vegeCals;
            ViewData["Others"] = othersCals;

            UpdateHealth();
            
            return View();
        }

        // Check if user exceed monthly calories
        private int ExceedCals(int monthCals, int famCals)
        {
            if(monthCals > famCals)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        // update health of user
        private void UpdateHealth()
        {
            int accID = (int)HttpContext.Session.GetInt32("AccID");
            List<Person> personList = personContext.GetAllPerson(accID);
            int totalCal = CalculateTotalCalories(personList);

            List<int> doesExceed = new List<int>();

            // Get total calories of items across the months for the year
            List<ShoppingBag> bagListJan = shoppingBagContext.GetBagsOfMonth(accID, 1);
            int janCal = CalculateMonthCalories(bagListJan);
            doesExceed.Add(ExceedCals(janCal, totalCal));

            List<ShoppingBag> bagListFeb = shoppingBagContext.GetBagsOfMonth(accID, 2);
            int febCal = CalculateMonthCalories(bagListFeb);
            doesExceed.Add(ExceedCals(febCal, totalCal));

            List<ShoppingBag> bagListMar = shoppingBagContext.GetBagsOfMonth(accID, 3);
            int marCal = CalculateMonthCalories(bagListMar);
            doesExceed.Add(ExceedCals(marCal, totalCal));

            List<ShoppingBag> bagListApr = shoppingBagContext.GetBagsOfMonth(accID, 4);
            int aprCal = CalculateMonthCalories(bagListApr);
            doesExceed.Add(ExceedCals(aprCal, totalCal));

            List<ShoppingBag> bagListMay = shoppingBagContext.GetBagsOfMonth(accID, 5);
            int mayCal = CalculateMonthCalories(bagListMay);
            doesExceed.Add(ExceedCals(mayCal, totalCal));

            List<ShoppingBag> bagListJun = shoppingBagContext.GetBagsOfMonth(accID, 6);
            int junCal = CalculateMonthCalories(bagListJun);
            doesExceed.Add(ExceedCals(junCal, totalCal));

            List<ShoppingBag> bagListJul = shoppingBagContext.GetBagsOfMonth(accID, 7);
            int julCal = CalculateMonthCalories(bagListJul);
            doesExceed.Add(ExceedCals(julCal, totalCal));

            List<ShoppingBag> bagListAug = shoppingBagContext.GetBagsOfMonth(accID, 8);
            int augCal = CalculateMonthCalories(bagListAug);
            doesExceed.Add(ExceedCals(augCal, totalCal));

            List<ShoppingBag> bagListSept = shoppingBagContext.GetBagsOfMonth(accID, 9);
            int septCal = CalculateMonthCalories(bagListSept);
            doesExceed.Add(ExceedCals(septCal, totalCal));

            List<ShoppingBag> bagListOct = shoppingBagContext.GetBagsOfMonth(accID, 10);
            int octCal = CalculateMonthCalories(bagListOct);
            doesExceed.Add(ExceedCals(octCal, totalCal));

            List<ShoppingBag> bagListNov = shoppingBagContext.GetBagsOfMonth(accID, 11);
            int novCal = CalculateMonthCalories(bagListNov);
            doesExceed.Add(ExceedCals(novCal, totalCal));

            List<ShoppingBag> bagListDec = shoppingBagContext.GetBagsOfMonth(accID, 12);
            int decCal = CalculateMonthCalories(bagListDec);
            doesExceed.Add(ExceedCals(decCal, totalCal));

            List<int> calForTheYearList = new List<int>() { janCal, febCal, marCal, aprCal, mayCal, junCal, julCal, augCal, septCal, octCal, novCal, decCal };
            ViewData["ThisYearCals"] = calForTheYearList;

            int totalHealth = 12;
            foreach (int i in doesExceed)
            {
                totalHealth -= i;
            }
            Account user = accountContext.GetDetails(accID);
            user.Health = totalHealth;
            accountContext.Update(user);
        }

        public IActionResult LandingMain()
        {
            // Authenticate user
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "User"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // GET: HomeController/LogOut
        public ActionResult LogOut()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        // Calculate total calories from all shopping bag
        private int CalculateCalTotal(List<ItemViewModel> itemVMList)
        {
            int totalCal = 0;
            foreach (ItemViewModel i in itemVMList)
            {
                int calSubTotal = i.CalSubTotal;
                totalCal += calSubTotal;
            }
            return totalCal;
        }

        // Calculate calories of each person
        private int CalculateTotalCalories(List<Person> personList)
        {
            List<PersonViewModel> pvm = MapToPersonVM(personList);
            int totalCals = 0;
            foreach (PersonViewModel p in pvm)
            {
                if(p.MaxCal == 0)
                {
                    totalCals += p.DerivedMaxCal;
                }
                else if(p.MaxCal > 0)
                {
                    totalCals += p.MaxCal;
                }
            }

            DateTime today = DateTime.Today;
            int totalDaysThisMonth = DateTime.DaysInMonth(today.Year, today.Month);
            return totalCals * totalDaysThisMonth;
        }

        public List<PersonViewModel> MapToPersonVM(List<Person> personList)
        {
            List<PersonViewModel> personVMList = new List<PersonViewModel>();
            foreach (Person person in personList)
            {
                PersonViewModel personVM = new PersonViewModel
                {
                    UserID = person.UserID,
                    UserName = person.UserName,
                    Age = person.GetAge(),
                    DerivedMaxCal = person.CalculateBMR(),
                    MaxCal = person.MaxCal.Value
                };
                personVMList.Add(personVM);
            }
            return personVMList;
        }

        // calculate the calories of the month from the shopping bags
        private int CalculateMonthCalories(List<ShoppingBag> bagList)
        {
            int cal = 0;
            List<ShoppingBagViewModel> shoppingBagViewModelList = new List<ShoppingBagViewModel>();
            foreach (ShoppingBag bag in bagList)
            {
                ShoppingBagViewModel shoppingBagVM = MapToShoppingBagVM(bag);
                shoppingBagViewModelList.Add(shoppingBagVM);
            }
            foreach (ShoppingBagViewModel bag in shoppingBagViewModelList)
            {
                cal += bag.totalCals;
            }
            return cal;
        }

        // Return item view model list from shopping bag ID
        public List<ItemViewModel> MapToItemVM(int shoppingBagID)
        {
            List<ItemViewModel> itemVMList = new List<ItemViewModel>();
            List<ShoppingBagItem> shoppingBagItemList = shoppingBagItemContext.GetAllShoppingBagItem(shoppingBagID);
            foreach (ShoppingBagItem s in shoppingBagItemList)
            {
                int itemID = s.ItemID;
                Item item = itemContext.GetDetails(itemID);
                ItemViewModel itemViewModel = new ItemViewModel
                {
                    ItemID = item.ItemID,
                    ShoppingBagID = s.ShoppingBagID,
                    ItemName = item.ItemName,
                    Category = item.Category,
                    Cal = item.Cal,
                    Qty = s.Qty,
                    CalSubTotal = item.Cal * s.Qty
                };
                itemVMList.Add(itemViewModel);
            }
            return itemVMList;
        }

        // Return shopping bag view model from shopping bag
        public ShoppingBagViewModel MapToShoppingBagVM(ShoppingBag shoppingBag)
        {
            List<ItemViewModel> itemVMList = MapToItemVM(shoppingBag.ShoppingBagID);
            ShoppingBagViewModel shoppingBagViewModel = new ShoppingBagViewModel
            {
                ShoppingBagID = shoppingBag.ShoppingBagID,
                BagName = shoppingBag.BagName,
                CreatedAt = shoppingBag.CreatedAt,
                BagDescription = shoppingBag.BagDescription,
                totalCals = CalculateCalTotal(itemVMList)
            };
            return shoppingBagViewModel;
        }
    }
}