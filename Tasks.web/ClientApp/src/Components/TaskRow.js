import { cssNumber } from 'jquery';
import React, { useState, useEffect } from 'react';
import getAxios from '../AuthAxios';

const TaskRow = ({ task }) => {

    const [user, setUser] = useState({firstName: '', lastName: ''});

    const onMarkPendingClick = async () => {
        task.status = 'Pending';
        await getAxios().post('/api/tasks/updatetaskstatus', task);
   
    }

    const onMarkDoneClick = async () => {
        task.status = 'Done';
        await getAxios().post('/api/tasks/markdone', task);
   
    }

    const getUserName = async () => {
        const {data} = await getAxios().get(`/api/account/getuserbyid?userId=${task.userId}`);
        setUser(data);
    }

    const getButton = status => {
        if(status === 'Incomplete' || status === 0){
            return(
                <button onClick={onMarkPendingClick} className="btn btn-info doing">I'm doing this one!</button>
            )
        }
        else if((status === 'Pending' && task.isThisUser) || (status === 1 && task.isThisUser)){
            return(
                <button onClick={onMarkDoneClick} className="btn btn-success">I'm done!</button>
            )
        }
        else if((status === 'Pending' && !task.isThisUser) || (status === 1 && !task.isThisUser)){
            getUserName();
            return(
                <button className="btn btn-warning" disabled={true}>{user.firstName} {user.lastName} is doing this</button>
            )
        }
     }

    return (
        <tr>
            <td>{task.name}</td>
            <td> {getButton(task.status)} </td>
        </tr>
    )
}
export default TaskRow;