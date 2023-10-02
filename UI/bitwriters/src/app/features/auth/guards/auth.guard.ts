import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { AuthService } from '../services/auth.service';
import jwt_decode from 'jwt-decode';

export const authGuard: CanActivateFn = (route, state) => {

  const cookieService = inject(CookieService);
  const authService = inject(AuthService);
  const router = inject(Router);
  const user = authService.getUser();

  //check for jwt token
  let token = cookieService.get('Authorization');

  if (token && user) {
    token = token.replace('Bearer ', '');
    const decodedToken: any = jwt_decode(token);

    //check if token has expired
    const expirationDate = decodedToken.exp * 1000;
    const currentTime = new Date().getTime();

    if (expirationDate < currentTime) {
      //logout
      authService.logout();

      //when they login, return them back to the current url/page
      return router.createUrlTree(['/login'], { queryParams: { returnUrl: state.url } });
    }
    else {
      //token is still valid
      if (user.roles.includes('Writer')) {
        return true;
      }
      else {
        //logout
        authService.logout();
        alert("Unauthorized");
        return false;
      }
    }
  }
  else {
    //logout
    authService.logout();

    //when they login, return them back to the current url/page
    return router.createUrlTree(['/login'], { queryParams: { returnUrl: state.url } });
  }
};
