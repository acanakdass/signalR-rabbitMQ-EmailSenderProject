import React, { useEffect, useState } from 'react';
import * as signalR from '@microsoft/signalr';
import axios from 'axios';

import { toast } from 'react-toastify';


const SendEmailForm = () => {

   const [email, setEmail] = useState('');
   const [content, setContent] = useState('');
   const [connectionId, setConnectionId] = useState();
   useEffect(() => {
      let connection = new signalR.HubConnectionBuilder()
         .withUrl("https://localhost:5001/messageHub")
         .build();
      connection.start().then(res => {
         console.log(connection.connectionId);
         setConnectionId(connection.connectionId);
      });

      connection.on("receiveMessage", data => {
         console.log('data from receiveMEssage method');
         console.log(data);
         toast(data);
      });
   }, []);

   const sendEmail = () => {
      axios.post("https://localhost:5001/api/message", {
         Email: email,
         Message: content,
         CallerConnectionId: connectionId
      }).then(res => {
         console.log(res.data)
      }).catch(er => {
         console.log(er);
      })
   }
   return (
      <div >
         <div className="App container p-5">
            <div className="m-3">
               <label className="form-label">Email address</label>
               <input onChange={e => setEmail(e.target.value)} type="email" className="form-control" placeholder="name@example.com" />
            </div>
            <div className="m-3">
               <label className="form-label">Content</label>
               <textarea onChange={e => setContent(e.target.value)} className="form-control" rows="3"></textarea>
            </div>
            <div>
               <button onClick={() => sendEmail()} className="btn btn-primary w-50" type="button">Send Email</button>
            </div>

         </div>
      </div >
   )
};

export default SendEmailForm;
