import { NgModule, provideBrowserGlobalErrorListeners, provideZonelessChangeDetection } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing-module';
import { App } from './components/original/app';
import { Turki } from './components/turki/turki';
import { FormsModule } from '@angular/forms';
import { Shehab } from './components/shehab/shehab';

@NgModule({
  declarations: [
    Turki,
    App,
    Shehab,
    
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule
  ],
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZonelessChangeDetection()
  ],
  bootstrap: [Turki]
})
export class AppModule { }
