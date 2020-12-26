import { AccountService } from './../_services/account.service';
import { prepareEventListenerParameters } from '@angular/compiler/src/render3/view/template';
import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  // @Input gets the user from the parent component Home.

  @Output() cancelRegister = new EventEmitter();
  model: any = {};
  emitValue: boolean;

  constructor(private injAccService : AccountService, private injtoastrService: ToastrService) { }

  ngOnInit(): void {
}

  register() {
    this.injAccService.register(this.model).subscribe(resp => {
      this.cancel()
      console.log(resp);
    }, error => {
      console.log(error)
      this.injtoastrService.error(error.error)
    })
  }

  cancel() {
    this.cancelRegister.emit(false);
  }

}
