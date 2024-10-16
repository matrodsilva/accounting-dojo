import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet,CommonModule, FormsModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'my-app';

  public processedArchives = [""];

  usuario: string = "";

  private _hubConnection!: HubConnection;

  constructor() {  
      
  } 

  connectarUsuario(){
    this.createConnection();  
    this.registerOnServerEvents();  
    this.startConnection();
  }

  connectToGroup(usuario: string) {  
    this._hubConnection.invoke('Connect', usuario);  
  }  
  
  private createConnection() {  
    this._hubConnection = new HubConnectionBuilder()  
      .withUrl('http://localhost:5296/archivehub')  
      .build();  
  }  
  
  private startConnection(): void {  
    this._hubConnection  
      .start()  
      .then(() => {   
       
          this.connectToGroup(this.usuario);
        
      })  
      .catch(() => {  
        setTimeout( () => { this.startConnection(); }, 5000);  
      });  
  }  
  
  private registerOnServerEvents(): void {  
    this._hubConnection.on('ArchiveStatus', (data: string) => {
      this.processedArchives.push(data);    
    });  
  }  
}
