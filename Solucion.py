import threading
import time

# Creamos dos recursos (locks)
lock1 = threading.Lock()
lock2 = threading.Lock()

def task1():
    print("Tarea 1: Intentando obtener lock1...")
    lock1.acquire()
    print("Tarea 1: Obtuvo lock1, ahora intentando obtener lock2...")
    time.sleep(1)
    
    lock2.acquire()
    print("Tarea 1: Obtuvo lock2")
    
    lock2.release()
    lock1.release()
    print("Tarea 1: Finalizada")

def task2():
    print("Tarea 2: Intentando obtener lock1...")
    lock1.acquire()
    print("Tarea 2: Obtuvo lock1, ahora intentando obtener lock2...")
    time.sleep(1)
    
    lock2.acquire()
    print("Tarea 2: Obtuvo lock2")
    
    lock2.release()
    lock1.release()
    print("Tarea 2: Finalizada")

# Creamos los hilos para las dos tareas
thread1 = threading.Thread(target=task1)
thread2 = threading.Thread(target=task2)

# Iniciamos los hilos
thread1.start()
thread2.start()

# Esperamos a que ambos hilos terminen
thread1.join()
thread2.join()
