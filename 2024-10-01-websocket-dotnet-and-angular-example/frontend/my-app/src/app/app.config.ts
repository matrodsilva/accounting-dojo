import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { NgModule } from '@angular/core';
import { routes } from './app.routes';
import { BrowserModule, provideClientHydration } from '@angular/platform-browser';

export const appConfig: ApplicationConfig = {
  
  providers: [provideRouter(routes), provideClientHydration()],
};
