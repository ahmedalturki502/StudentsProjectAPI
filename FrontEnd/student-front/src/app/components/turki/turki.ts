import { Component } from '@angular/core';

@Component({
  selector: 'app-turki',
  standalone: false,
  templateUrl: './turki.html',
  styleUrl: './turki.css'
})
export class Turki {
  formData:any = {
    id: '',
    name: '',
    address: ''
  };


  DoAction() {
    console.log('Form submitted:', this.formData);
    this.formData.id='';
    this.formData.name='';
    this.formData.address='';
    alert("Hello turki");
  }
}
