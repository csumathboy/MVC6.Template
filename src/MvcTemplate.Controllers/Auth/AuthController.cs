﻿using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using MvcTemplate.Components.Alerts;
using MvcTemplate.Components.Mail;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Objects;
using MvcTemplate.Resources.Views.Administration.Accounts.AccountView;
using MvcTemplate.Services;
using MvcTemplate.Validators;
using System;
using System.Threading.Tasks;

namespace MvcTemplate.Controllers
{
    [AllowAnonymous]
    public class AuthController : ValidatedController<IAccountValidator, IAccountService>
    {
        public IMailClient MailClient { get; }

        public AuthController(IAccountValidator validator, IAccountService service, IMailClient mailClient)
            : base(validator, service)
        {
            MailClient = mailClient;
        }

        [HttpGet]
        public ActionResult Register()
        {
            if (Service.IsLoggedIn(User))
                return RedirectToDefault();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([BindExcludeId] AccountRegisterView account)
        {
            if (Service.IsLoggedIn(User))
                return RedirectToDefault();

            if (!Validator.CanRegister(account))
                return View(account);

            Service.Register(account);

            Alerts.Add(AlertType.Success, Messages.SuccessfulRegistration);

            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Recover()
        {
            if (Service.IsLoggedIn(User))
                return RedirectToDefault();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Recover(AccountRecoveryView account)
        {
            if (Service.IsLoggedIn(User))
                return RedirectToDefault();

            if (!Validator.CanRecover(account))
                return View(account);

            String token = Service.Recover(account);
            if (token != null)
            {
                String url = Url.Action("Reset", "Auth", new { token }, Request.Scheme);

                await MailClient.SendAsync(
                    account.Email,
                    Messages.RecoveryEmailSubject,
                    String.Format(Messages.RecoveryEmailBody, url));
            }

            Alerts.Add(AlertType.Info, Messages.RecoveryInformation, 0);

            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Reset(String token)
        {
            if (Service.IsLoggedIn(User))
                return RedirectToDefault();

            AccountResetView account = new AccountResetView();
            account.Token = token;

            if (!Validator.CanReset(account))
                return RedirectToAction("Recover");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reset(AccountResetView account)
        {
            if (Service.IsLoggedIn(User))
                return RedirectToDefault();

            if (!Validator.CanReset(account))
                return RedirectToAction("Recover");

            Service.Reset(account);

            Alerts.Add(AlertType.Success, Messages.SuccessfulReset);

            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Login(String returnUrl)
        {
            if (Service.IsLoggedIn(User))
                return RedirectToLocal(returnUrl);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AccountLoginView account, String returnUrl)
        {
            if (Service.IsLoggedIn(User))
                return RedirectToLocal(returnUrl);

            if (!Validator.CanLogin(account))
                return View(account);

            Service.Login(HttpContext.Authentication, account.Username);

            return RedirectToLocal(returnUrl);
        }

        [HttpGet]
        public RedirectToActionResult Logout()
        {
            Service.Logout(HttpContext.Authentication);

            return RedirectToAction("Login");
        }
    }
}
