import React,{useState,useEffect } from 'react';
import { Link, useHistory} from 'react-router-dom';
import { useAuthContext } from '../AuthContext';
import getAxios from '../AuthAxios';
import Joke  from '../components/Joke';


const ViewAll = () => {

   // const { user } = useAuthContext();
    const[jokes, setJokes]=useState('');
   // const[like,setLike]=useState(0);
   // const[dislike, setDislike]=useState(0);

  //  const{punchLine,setUp,id, likes, dislikes}=joke;
  
   
    useEffect(() => {

        const getJokes = async () => {

            const { data } = await getAxios().get('/api/jokes/getjokes');
          
             await setJokes(data);
             console.log(jokes);
       
        }
        getJokes();
   
        
      }, []);

    return <>
     <div className='col-md-6 offset-md-3'>
  {jokes&&jokes.map(j =>
   <Joke key={j.id}
         joke={j}
    />)}

    </div>

    </>

}
export default ViewAll;